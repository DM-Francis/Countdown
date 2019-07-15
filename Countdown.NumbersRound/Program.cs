using Countdown.NumbersRound.PrefixNotationBased;
using System;
using System.Collections.Generic;
using System.Text;

namespace Countdown.NumbersRound
{
    public static class Program
    {
        public static void Main()
        {
            int target = 150;
            var availableNums = new List<int> { 1,2,3,4,5,6 };

            var solver = new PrefixSolver();
            var result = solver.GetPossibleSolutions(target, availableNums);

            foreach(var sol in result.Solutions)
            {
                Console.WriteLine(sol);
            }
        }
    }
}
