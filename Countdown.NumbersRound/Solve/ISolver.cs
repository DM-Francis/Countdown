using System.Collections.Generic;

namespace Countdown.NumbersRound.Solve
{
    public interface ISolver
    {
        SolveResult GetPossibleSolutions(int target, List<int> availableNums);
    }
}