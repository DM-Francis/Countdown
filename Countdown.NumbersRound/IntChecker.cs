using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound
{
    internal class Checker<T> : ExpressionVisitor
    {
        private List<T> usedVals;

        public List<T> GetUsedInts(System.Linq.Expressions.Expression expression)
        {
            usedVals = new List<T>();
            Visit(expression);
            return usedVals;
        }

        public bool ContainsElementInList(System.Linq.Expressions.Expression expression, List<T> vals)
        {
            usedVals = new List<T>();
            Visit(expression);

            return usedVals.Intersect(vals).Any();
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            usedVals.Add((T)node.Value);
            return base.VisitConstant(node);
        }
    }
}
