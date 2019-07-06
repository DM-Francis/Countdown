using Combinatorics.Collections;
using SF = MathNet.Numerics.SpecialFunctions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        private List<(Expression exp, float result)> _solutions = new List<(Expression, float)>();
        private int _target;
        private int _totalSearched;
        private int _validCount;

        public Solver(ILogger logger)
        {
            _logger = logger;
        }

        public List<string> GetPossibleSolutions(int target, List<int> availableNums)
        {
            _target = target;
            _totalSearched = 0;
            _validCount = 0;

            List<float> availableNumsFloat = availableNums.Select(i => (float)i).ToList();

            int N = availableNums.Count;
            for (int i = 1; i <= N; i++)
            {
                TestExpressionsOfLength(i, availableNumsFloat);
            }

            // Dedupe solutions
            var solutionStrings = _solutions.Select(sol => $"{sol.exp} = {sol.result}").Distinct().ToList();


            _logger.LogInformation("Available numbers = {availableNums}", string.Join(',', availableNums));
            _logger.LogInformation("Target = {target}", _target);

            _logger.LogInformation("Total searched = {totalSearched}", _totalSearched);
            _logger.LogInformation("Valid expressions found = {validCount}", _validCount);
            _logger.LogInformation("{solutionCount} solutions found", solutionStrings.Count);
            _logger.LogInformation("{solutions}", solutionStrings);

            return solutionStrings;
        }

        private void TestExpressionsOfLength(int N, List<float> availableNums)
        {
            var variations = new Variations<float>(availableNums, N, GenerateOption.WithoutRepetition);

            foreach (var variation in variations)
            {
                foreach (var exp in GetPossibleTrees(N))
                {
                    var evaluator = new Evaluator(variation);
                    var result = evaluator.Evaluate(exp);
                    _totalSearched++;

                    if (result == _target)
                    {
                        var populator = new Populator(variation);
                        var newExp = populator.Populate(exp);
                        _solutions.Add((newExp, result));
                    }

                    if (!float.IsNaN(result))
                    {
                        _validCount++;
                    }
                }
            }
        }

        // Method to create possible expression trees with N leaves.
        private List<Expression> GetPossibleTrees(int N)
        {
            // Don't put any numbers in these.  Only create the bracket/operation structure.

            // Check cache first
            if (_expressionCache.ContainsKey(N))
            {
                _expressionCache.TryGetValue(N, out List<Expression> cacheResult);

                return cacheResult;
            }

            var resultList = new List<Expression>();

            if (N == 1)
            {
                resultList.Add(Expression.Constant((float)1));
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

        private int GetTotalCombinations(List<float> availableNumbers)
        {
            int N = availableNumbers.Count;
            double total = 0;

            for (int n = 1; n <= N; n++)
            {
                total += Math.Pow(4, n - 1) * (SF.Factorial(N) / (n * SF.Factorial(N - n))) * SF.Binomial(2 * n - 2, n - 1);
            }

            return (int)total;
        }
    }
}
