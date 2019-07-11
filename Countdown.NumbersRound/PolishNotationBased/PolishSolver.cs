using System;
using System.Collections.Generic;
using System.Text;

namespace Countdown.NumbersRound.PolishNotationBased
{
    public sealed class PolishSolver : ISolver
    {
        public SolveResult GetPossibleSolutions(int target, List<int> availableNums)
        {
            return new SolveResult { ClosestDiff = 0, Solutions = new List<string>() };
        }
    }
}
