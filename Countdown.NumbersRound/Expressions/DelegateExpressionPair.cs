using System;
using System.Linq.Expressions;

namespace Countdown.NumbersRound.Expressions
{
    internal class DelegateExpressionPair
    {
        public Func<double[], double> Delegate { get; }
        public Expression Expression { get; }

        public DelegateExpressionPair(Func<double[], double> del, Expression expression)
            => (Delegate, Expression) = (del, expression);
    }
}
