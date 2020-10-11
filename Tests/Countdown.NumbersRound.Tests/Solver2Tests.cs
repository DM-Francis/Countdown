using Countdown.NumbersRound.Solve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Countdown.NumbersRound.Tests
{
    public class Solver2Tests
    {
        public static IEnumerable<object[]> ProblemsWithExactSolution
        {
            get
            {
                yield return new object[] { 561, new List<int> { 25, 50, 75, 100, 1, 3 } };
                yield return new object[] { 100, new List<int> { 1, 2, 3, 4, 5, 6 } };
                yield return new object[] { 903, new List<int> { 5, 2, 1, 7, 7, 8 } };
            }
        }

        [Theory]
        [MemberData(nameof(ProblemsWithExactSolution))]
        public void CanFindExactSolutions(int target, List<int> availableNums)
        {
            // Assemble
            var solver = new Solver2(target, availableNums);

            // Act
            var result = solver.Solve();

            // Assert
            Assert.Equal(0, result.ClosestDiff);
            Assert.NotEmpty(result.Solutions);
        }

        [Fact]
        public void CanFindClosestSolutions()
        {
            // Assemble
            var availableNums = new List<int> { 2, 3, 4, 5, 8, 10 };
            var solver = new Solver2(933, availableNums);

            // Act
            var result = solver.Solve();

            // Assert
            Assert.Equal(1, result.ClosestDiff);
            Assert.Equal(2, result.Solutions.Count);
        }

        [Fact]
        public void WorksWhenSolutionIsAlreadyInAvailableNumbers()
        {
            // Assemble
            var availableNums = new List<int> { 1, 2, 3, 4, 5, 100 };
            var solver = new Solver2(100, availableNums);

            // Act
            var result = solver.Solve();

            // Assert
            Assert.Equal(0, result.ClosestDiff);
            Assert.NotEmpty(result.Solutions);
        }
    }
}
