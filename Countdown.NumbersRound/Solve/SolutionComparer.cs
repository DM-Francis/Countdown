using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Countdown.NumbersRound.Solve
{
    internal class SolutionComparer : IEqualityComparer<List<Operation>>
    {
        public bool Equals(List<Operation> x, List<Operation> y)
        {
            if (x.Equals(y))
                return true;

            if (x is null || y is null || x.Count != y.Count)
                return false;

            IEnumerable<double> xIntermediateValues = x.Select(x => x.Operator.Evaluate(x.A, x.B));
            IEnumerable<double> yIntermediateValues = y.Select(y => y.Operator.Evaluate(y.A, y.B));

            return xIntermediateValues.SequenceEqual(yIntermediateValues);
        }

        public int GetHashCode(List<Operation> obj)
        {
            int hashCode = 1239679;

            for (int i = 0; i < obj.Count; i++)
            {
                Operation operation = obj[i];
                double intermediateValue = operation.Operator.Evaluate(operation.A, operation.B);
                hashCode ^= intermediateValue.GetHashCode();
                hashCode ^= i * 37;
            }

            return hashCode;
        }
    }
}
