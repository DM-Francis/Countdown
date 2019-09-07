using System.Collections.Generic;

namespace Countdown.NumbersRound.Solve
{
    public class SolveResult
    {
        public int ClosestDiff { get; }
        public List<string> Solutions { get; }

        public SolveResult(int closestDiff, List<string> solutions)
            => (ClosestDiff, Solutions) = (closestDiff, solutions);
    }
}
