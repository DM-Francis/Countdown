using Combinatorics.Collections;
using SF = MathNet.Numerics.SpecialFunctions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Countdown.NumbersRound
{
    public class Solver : ISolver
    {
        private static readonly List<ExpressionType> _operations = new List<ExpressionType> { ExpressionType.Add, ExpressionType.Subtract, ExpressionType.Multiply, ExpressionType.Divide };

        private readonly ILogger _logger;

        private readonly Dictionary<int, List<Expression>> _expressionCache = new Dictionary<int, List<Expression>>();
        private List<Solution> _solutions = new List<Solution>();
        private int _currentClosestDiff;

        private int _target;
        private int _totalSearched;
        private int _validCount;

        public Solver(ILogger<Solver> logger)
        {
            _logger = logger;
        }

        public SolveResult GetPossibleSolutions(int target, List<int> availableNums)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            _target = target;
            _totalSearched = 0;
            _validCount = 0;

            List<double> availableNumsdouble = availableNums.Select(i => (double)i).ToList();

            int N = availableNums.Count;
            _currentClosestDiff = target;
            for (int i = 1; i <= N; i++)
            {
                TestExpressionsOfLength(i, availableNumsdouble);
            }
            List<string> solutionStrings = RenderSolutionExpressions();
            stopWatch.Stop();

            _logger.LogInformation("Available numbers = {availableNums}", string.Join(',', availableNums));
            _logger.LogInformation("Target = {target}", _target);

            _logger.LogInformation("Total searched = {totalSearched}", _totalSearched);
            _logger.LogInformation("Valid expressions found = {validCount}", _validCount);
            _logger.LogInformation("{solutionCount} solutions found", solutionStrings.Count);
            _logger.LogInformation("{solutions}", solutionStrings);
            _logger.LogInformation("Time taken: {timeTaken}", stopWatch.Elapsed.Duration().ToString());

            return new SolveResult { ClosestDiff = _currentClosestDiff, Solutions = solutionStrings };
        }

        private void TestExpressionsOfLength(int N, List<double> availableNums)
        {
            /*
            New algorithm:
            1. For each N, we will create a list of expressions, where each expression has N parameters.
            2. Compile each expression into a lambda with N parameters.
            3. Evaluate each lambda with all possible number combinations.
            */

            var variations = new Variations<double>(availableNums, N, GenerateOption.WithoutRepetition);
            var variationIterable = variations.Select(l => l.ToArray()).ToArray();

            foreach (var exp in GetPossibleTrees(N))
            {
                var paramExpression = Expression.Parameter(typeof(double[]));
                var populator = new Populator(paramExpression);
                Expression expWithParam = populator.Populate(exp);

                var lambda = Expression.Lambda<Func<double[], double>>(expWithParam, paramExpression);
                var del = lambda.Compile();

                foreach (var variation in variationIterable)
                {
                    double result = del.Invoke(variation);
                    _totalSearched++;

                    double diff = Math.Abs(_target - result);
                    if (diff == _currentClosestDiff)
                    {
                        AddResultToSolutions(exp, result, variation);
                    }
                    else if (diff < _currentClosestDiff)
                    {
                        _solutions.Clear();
                        _currentClosestDiff = (int)diff;
                        AddResultToSolutions(exp, result, variation);
                    }

                    if (!double.IsNaN(result))
                    {
                        _validCount++;
                    }
                }
            }
        }

        private void AddResultToSolutions(Expression exp, double result, double[] variation)
        {
            _solutions.Add(new Solution() { Expression = exp, Result = result, Params = new List<double>(variation)});
        }

        // Method to create possible expression trees with N leaves.  Each expression should take N parameters.
        private List<Expression> GetPossibleTrees(int N)
        {
            // Don't put any numbers in these.  Only create the bracket/operation structure. Each number is a parameter.
            // Check cache first
            if (_expressionCache.ContainsKey(N))
            {
                _expressionCache.TryGetValue(N, out List<Expression> cacheResult);

                return cacheResult;
            }

            var resultList = new List<Expression>();

            if (N == 1)
            {
                resultList.Add(Expression.Parameter(typeof(double)));
            }
            else
            {
                for (int x = 1; x < N; x++)
                {
                    foreach (var leftTree in GetPossibleTrees(x))
                    {
                        foreach (var rightTree in GetPossibleTrees(N - x))
                        {
                            var possibleExpressions = GetBinaryExpressions(leftTree, rightTree, _operations);
                            resultList.AddRange(possibleExpressions);
                        }
                    }
                }
            }

            _expressionCache.Add(N, resultList);

            return resultList;
        }

        // Returns a list of binary expressions using the 2 given expresssions and the types of operation wanted.
        private List<Expression> GetBinaryExpressions(Expression left, Expression right, List<ExpressionType> operations)
        {
            var output = new List<Expression>();

            foreach (var operation in operations)
            {
                var newExpr = Expression.MakeBinary(operation, left, right);
                output.Add(newExpr);
            }

            return output;
        }

        private List<string> RenderSolutionExpressions()
        {
            List<string> solStrings = new List<string>();
            foreach (var sol in _solutions)
            {
                // Populate values in the expression
                var populator = new Populator(sol.Params);
                var finalExp = populator.Populate(sol.Expression);

                // Write expression as string
                solStrings.Add($"{finalExp} = {sol.Result}");
            }

            return solStrings.Distinct().ToList();
        }
    }
}
