using Countdown.NumbersRound.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound.Solve
{
    public class ExpressionFactory
    {
        private static readonly IReadOnlyCollection<ExpressionType> Operations
            = new List<ExpressionType>() { ExpressionType.Add, ExpressionType.Subtract, ExpressionType.Multiply, ExpressionType.Divide };

        private readonly IDelegateCache _delegateCache;
        private readonly IExpressionCache _expressionCache;

        public ExpressionFactory(IDelegateCache delegateCache, IExpressionCache expressionCache)
        {
            _delegateCache = delegateCache;
            _expressionCache = expressionCache;
        }

        public List<DelegateExpressionPair> GetAllDelegateExpressionPairs(int N)
        {
            if (_delegateCache.TryGetValue(N, out List<DelegateExpressionPair> cacheResult))
            {
                return cacheResult;
            }

            var expressions = CreateExpressionsOfSize(N);
            var outputList = CreateDelegatePairsFromExpressions(expressions);

            _delegateCache.Add(N, outputList);

            return outputList;
        }

        private IEnumerable<Expression> CreateExpressionsOfSize(int N)
        {
            if (_expressionCache.TryGetValue(N, out IEnumerable<Expression>? expressions))
            {
                return expressions;
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
                    foreach (Expression leftTree in CreateExpressionsOfSize(x))
                    {
                        foreach (Expression rightTree in CreateExpressionsOfSize(N - x))
                        {
                            var possibleExpressions = GetBinaryExpressions(leftTree, rightTree);
                            expressionList.AddRange(possibleExpressions);
                        }
                    }
                }
            }

            _expressionCache.Add(N, expressionList);

            return expressionList;
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

        private static List<DelegateExpressionPair> CreateDelegatePairsFromExpressions(IEnumerable<Expression> expressionList)
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
    }
}
