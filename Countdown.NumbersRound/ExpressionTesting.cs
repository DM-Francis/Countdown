using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Countdown.NumbersRound
{
    internal static class ExpressionTesting
    {
        private static Dictionary<int, List<Expression>> _expressionCache = new Dictionary<int, List<Expression>>();
        private static List<ExpressionType> _operations = new List<ExpressionType> { ExpressionType.Add, ExpressionType.Subtract, ExpressionType.Multiply, ExpressionType.Divide };
        private static int _N = 6;
        private static int _target = 677;
        private static List<float> _availableNumbers = new List<float> { 75, 6, 1, 8, 9, 7 };
        private static bool _finished;
        private static Checker<float> _checker = new Checker<float>();

        public static void TestExpression()
        {
            var expressions = new List<Expression>();
            for (int i = 1; i <= 6; i++)
            {
                _N = i;
                expressions.AddRange(GetPossibleTrees(_N));
            }

            //foreach(var exp in expressions)
            //{
            //    Console.WriteLine(exp);
            //}

            Console.WriteLine(expressions.Count);
        }

        // Method to create possible expression trees with N leaves.
        public static List<Expression> GetPossibleTrees(int N)
        {
            // Assume the same binary operation: addition
            // Assume only one possible number: 1
            var resultList = new List<Expression>();

            // Check cache first
            if (_expressionCache.ContainsKey(N))
            {
                _expressionCache.TryGetValue(N, out List<Expression> cacheResult);
                return cacheResult;
            }

            if (N == 1)
            {
                resultList.AddRange(GetConstantExpressions(_availableNumbers));
            }
            else
            {
                for (int x = 1; x < N; x++)
                {
                    foreach (var leftTree in GetPossibleTrees(x))
                    {
                        foreach (var rightTree in GetPossibleTrees(N - x))
                        {
                            // Check for overlap in the trees.
                            if (!_checker.ContainsElementInList(rightTree, _checker.GetUsedInts(leftTree)))
                            {
                                var possibleExpressions = GetBinaryExpressions(leftTree, rightTree, _operations);
                                resultList.AddRange(possibleExpressions);
                            }
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

        // Method to create all possible constant expressions for a provided array of numbers
        public static List<Expression> GetConstantExpressions(List<float> numbers)
        {
            var constantExpressions = new List<Expression>();
            foreach (float num in numbers)
            {
                constantExpressions.Add(Expression.Constant(num));
            }

            return constantExpressions;
        }
    }
}
