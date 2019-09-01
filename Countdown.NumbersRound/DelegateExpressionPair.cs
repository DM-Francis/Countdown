using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound
{
    internal class DelegateExpressionPair
    {
        public Func<double[], double> Delegate { get; set; }
        public Expression Expression { get; set; } 
    }
}
