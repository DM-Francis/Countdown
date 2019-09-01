using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound.Expressions
{
    internal class DelegateExpressionPair
    {
        public Func<double[], double> Delegate { get; set; }
        public Expression Expression { get; set; }
    }
}
