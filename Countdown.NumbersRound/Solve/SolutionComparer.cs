using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Countdown.NumbersRound.Solve
{
    internal class SolutionComparer : IEqualityComparer<Solution>
    {
        public bool Equals(Solution x, Solution y)
        {
            if (x.Equals(y))
                return true;

            if (x is null || y is null || x.Operations.Count != y.Operations.Count)
                return false;

            var xNumbersUsed = x.NumbersUsed.OrderBy(n => n);
            var yNumbersUsed = y.NumbersUsed.OrderBy(n => n);
            IEnumerable<double> xIntermediateValues = x.Operations.Select(x => x.Operator.Evaluate(x.A, x.B));
            IEnumerable<double> yIntermediateValues = y.Operations.Select(y => y.Operator.Evaluate(y.A, y.B));

            return xIntermediateValues.SequenceEqual(yIntermediateValues)
                && xNumbersUsed.SequenceEqual(yNumbersUsed);
        }

        public int GetHashCode(Solution obj)
        {
            int hashCode = 1239679;

            for (int i = 0; i < obj.Operations.Count; i++)
            {
                Operation operation = obj.Operations[i];
                double intermediateValue = operation.Operator.Evaluate(operation.A, operation.B);
                hashCode ^= intermediateValue.GetHashCode();
                hashCode ^= i * 37;
            }

            foreach(int num in obj.NumbersUsed)
            {
                hashCode ^= num;
            }

            return hashCode;
        }
    }
}
