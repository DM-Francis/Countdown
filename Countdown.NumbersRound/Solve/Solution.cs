using System.Collections.Generic;
using System.Linq.Expressions;

namespace Countdown.NumbersRound.Solve
{
    internal class Solution
    {
        public Expression Expression { get; }
        public List<double> Parameters { get; }
        public double Result { get; }

        public Solution(Expression expression, List<double> parameters, double result)
            => (Expression, Parameters, Result) = (expression, parameters, result);
    }
}
