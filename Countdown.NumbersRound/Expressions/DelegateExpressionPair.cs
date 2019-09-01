using System;
using System.Linq.Expressions;

namespace Countdown.NumbersRound.Expressions
{
    internal class DelegateExpressionPair
    {
        public Func<double[], double> Delegate { get; set; }
        public Expression Expression { get; set; }
    }
}
