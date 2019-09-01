using System.Collections.Generic;
using System.Threading.Tasks;

namespace Countdown.NumbersRound.Solve
{
    public interface ISolver
    {
        SolveResult GetPossibleSolutions(int target, List<int> availableNums);
    }
}