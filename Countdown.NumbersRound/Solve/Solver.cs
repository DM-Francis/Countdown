using Combinatorics.Collections;
using Countdown.NumbersRound.Expressions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound.Solve
{
    public class Solver : ISolver
    {
        private static readonly IReadOnlyCollection<ExpressionType> Operations = new List<ExpressionType>() { ExpressionType.Add, ExpressionType.Subtract, ExpressionType.Multiply, ExpressionType.Divide };

        private readonly IDelegateCache _delegateCache;
        private readonly ILogger _logger;

        private readonly List<Solution> _solutions = new List<Solution>();
        private int _currentClosestDiff;
        private int _target;

        public Solver(ILogger<Solver> logger, IDelegateCache delegateCache)
        {
            _logger = logger;
            _delegateCache = delegateCache;
        }

        public SolveResult GetPossibleSolutions(int target, List<int> availableNums)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            _target = target;

            List<double> availableNumsDouble = availableNums.Select(i => (double)i).ToList();

            int N = availableNums.Count;
            _currentClosestDiff = target;
            for (int i = 1; i <= N; i++)
            {
                CheckExpressionsOfLength(i, availableNumsDouble);
            }

            List<string> solutionStrings = RenderSolutionExpressions(_solutions);
            stopWatch.Stop();

            _logger.LogInformation("Available numbers = {availableNums}", string.Join(",", availableNums));
            _logger.LogInformation("Target = {target}", _target);
            _logger.LogInformation("{solutionCount} solutions found", solutionStrings.Count);
            _logger.LogInformation("{solutions}", solutionStrings);
            _logger.LogInformation("Time taken: {timeTaken}", stopWatch.Elapsed.Duration().ToString());

            return new SolveResult(closestDiff: _currentClosestDiff, solutions: solutionStrings);
        }

        private void CheckExpressionsOfLength(int N, List<double> availableNums)
        {
            var variations = new Variations<double>(availableNums, N, GenerateOption.WithoutRepetition);
            var variationIterable = variations.Select(l => l.ToArray()).ToArray();

            foreach (var pair in GetAllDelegateExpressionPairs(N))
            {
                foreach (var variation in variationIterable)
                {
                    double result = pair.Delegate.Invoke(variation);
                    double diff = Math.Abs(_target - result);

                    if (diff == _currentClosestDiff && SolutionIsValid(pair.Expression, result, variation))
                    {
                        AddResultToSolutions(pair.Expression, result, variation);
                    }
                    else if (diff < _currentClosestDiff && SolutionIsValid(pair.Expression, result, variation))
                    {
                        _solutions.Clear();
                        _currentClosestDiff = (int)diff;
                        AddResultToSolutions(pair.Expression, result, variation);
                    }
                }
            }
        }

        private bool SolutionIsValid(Expression expression, double result, double[] availableNums)
        {
            if (result % 1 != 0)
            {
                return false;
            }

            var validator = new Validator(availableNums);
            var validatedResult = validator.CheckExpression(expression); // Returns double.NaN if invalid or a dupe

            return !double.IsNaN(validatedResult);
        }

        private void AddResultToSolutions(Expression exp, double result, double[] variation)
        {
            _solutions.Add(new Solution(expression: exp, parameters: new List<double>(variation), result: result));
        }

        private List<DelegateExpressionPair> GetAllDelegateExpressionPairs(int N)
        {
            if (_delegateCache.TryGetValue(N, out List<DelegateExpressionPair> cacheResult))
            {
                return cacheResult;
            }

            var expressionList = new List<Expression>();

            if (N == 1)
            {
                expressionList.Add(Expression.Parameter(typeof(double)));
            }
            else
            {
                for (int x = 1; x < N; x++)
                {
                    foreach (var leftTree in GetAllDelegateExpressionPairs(x).Select(p => p.Expression))
                    {
                        foreach (var rightTree in GetAllDelegateExpressionPairs(N - x).Select(p => p.Expression))
                        {
                            var possibleExpressions = GetBinaryExpressions(leftTree, rightTree);
                            expressionList.AddRange(possibleExpressions);
                        }
                    }
                }
            }

            var outputList = CreateDelegatePairsFromExpressions(expressionList);

            _delegateCache.Add(N, outputList);

            return outputList;
        }

        private static List<Expression> GetBinaryExpressions(Expression left, Expression right)
        {
            var output = new List<Expression>();

            foreach (var operation in Operations)
            {
                var newExpr = Expression.MakeBinary(operation, left, right);
                output.Add(newExpr);
            }

            return output;
        }

        private static List<DelegateExpressionPair> CreateDelegatePairsFromExpressions(List<Expression> expressionList)
        {
            var outputList = new List<DelegateExpressionPair>();
            var paramExpression = Expression.Parameter(typeof(double[]));
            var populator = new Populator(paramExpression);

            foreach (var exp in expressionList)
            {
                Expression expWithParam = populator.Populate(exp);

                var lambda = Expression.Lambda<Func<double[], double>>(expWithParam, paramExpression);
                var pair = new DelegateExpressionPair(del: lambda.Compile(), expression: exp);

                outputList.Add(pair);
            }

            return outputList;
        }

        private static List<string> RenderSolutionExpressions(IEnumerable<Solution> solutions)
        {
            List<string> solStrings = new List<string>();
            foreach (var sol in solutions)
            {
                // Populate values in the expression
                var populator = new Populator(sol.Parameters);
                var finalExp = populator.Populate(sol.Expression);

                // Write expression as string
                string expString = finalExp.ToString();

                // Remove unneeded parentheses
                if (finalExp.NodeType != ExpressionType.Constant) // Not a single number
                {
                    expString = expString.Substring(1, expString.Length - 2);
                }

                expString = expString.Replace('*', '×');
                solStrings.Add($"{expString} = {sol.Result}");
            }

            return solStrings.Distinct().ToList();
        }
    }
}
