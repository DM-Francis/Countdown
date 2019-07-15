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
            // Arrange
            int target = 2;
            var availableNums = new List<int> { 1, 1 };

            // Act
            var solver = new PrefixSolver();
            var result = solver.GetPossibleSolutions(target, availableNums);
        }
    }
}
