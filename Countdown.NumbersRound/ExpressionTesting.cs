using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using SF = MathNet.Numerics.SpecialFunctions;

namespace Countdown.NumbersRound
{
    internal static class ExpressionTesting
    {
        private static readonly Dictionary<int, List<Expression>> _expressionCache = new Dictionary<int, List<Expression>>();
        private static readonly List<ExpressionType> _operations = new List<ExpressionType> { ExpressionType.Add, ExpressionType.Subtract, ExpressionType.Multiply, ExpressionType.Divide };
        private static int _target;
        private static int _totalSearched;
        private static int _validCount;

        private static readonly List<float> _largeNumbers = new List<float> { 25, 50, 75, 100 };
        private static readonly List<float> _smallNumbers = new List<float> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10 };

        private static List<(Expression, float)> _solutions = new List<(Expression, float)>();

        public static void TestExpression()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var rng = new Random();
            _target = rng.Next(101, 999);

            _target = SmallPrimeUtility.PrimeTable.Where(p => p > 100 && p < 1000).OrderBy(_ => Guid.NewGuid()).First();

            int largeAmount = 1;
            int smallAmount = 6 - largeAmount;

            List<float> availableNums = new List<float>();
            availableNums.AddRange(_largeNumbers.OrderBy(_ => Guid.NewGuid()).Take(largeAmount));
            availableNums.AddRange(_smallNumbers.OrderBy(_ => Guid.NewGuid()).Take(smallAmount));

            availableNums = new List<float> { 50, 75, 100, 25, 1, 8 };
            _target = 841;

            Console.WriteLine($"Total combinations to check: {GetTotalCombinations(availableNums)}");

            int N = availableNums.Count;
            for (int i = 1; i <= N; i++)
            {
                TestExpressionsOfLength(i, availableNums);
            }

            // Dedupe solutions
            List<string> solutionStrings = _solutions.Select(sol => $"{sol.Item1} = {sol.Item2}").Distinct().ToList();

            stopWatch.Stop();

            Console.WriteLine($"Available numbers = {string.Join(',', availableNums)}");
            Console.WriteLine($"Target = {_target}");

            Console.WriteLine($"Total searched = {_totalSearched}");
            Console.WriteLine($"Valid expressions found = {_validCount}");
            Console.WriteLine($"{solutionStrings.Count} solutions found:");
            foreach (string solution in solutionStrings)
            {
                Console.WriteLine(solution);
            }
            Console.WriteLine($"Time taken: {stopWatch.Elapsed.TotalSeconds} seconds.");
        }

        private static void TestExpressionsOfLength(int N, List<float> availableNums)
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

                        if (_validCount % 1000 == 0)
                        {
                            Console.WriteLine($"Total: {_totalSearched} Valid: {_validCount}");
                        }
                    }
                }
            }
        }

        // Method to create possible expression trees with N leaves.
        public static List<Expression> GetPossibleTrees(int N)
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
        public static List<Expression> GetBinaryExpressions(Expression left, Expression right, List<ExpressionType> operations)
        {
            var output = new List<Expression>();

            foreach (var operation in operations)
            {
                var newExpr = Expression.MakeBinary(operation, left, right);
                output.Add(newExpr);
            }

            return output;
        }

        private static int GetTotalCombinations(List<float> availableNumbers)
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
