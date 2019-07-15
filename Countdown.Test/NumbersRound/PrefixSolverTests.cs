using Countdown.NumbersRound.PrefixNotationBased;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Countdown.Test.NumbersRound
{
    public class PrefixSolverTests
    {
        [Fact]
        public void GetsSolutionForTrivialExample()
        {
            // Arrange
            int target = 2;
            var availableNums = new List<int> { 1, 1 };

            // Act
            var solver = new PrefixSolver();
            var result = solver.GetPossibleSolutions(target, availableNums);

            // Assert
            Assert.Equal(0, result.ClosestDiff);
        }
    }
}
