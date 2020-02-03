using Countdown.NumbersRound.Solve;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Countdown.NumbersRound.Tests
{
    public class SolverTests
    {
        private Solver GetSolver()
        {
            return new Solver(new NullLogger<Solver>(), new DictionaryDelegateCache());
        }

        [Fact]
        public void CanSolveUsing2Numbers()
        {
            // Assemble
            var solver = GetSolver();

            // Act
            var sols = solver.GetPossibleSolutions(10, new List<int> { 5, 2 });

            // Assert
            Assert.Equal(0, sols.ClosestDiff);
            Assert.Contains("5 × 2 = 10", sols.Solutions);
        }

        [Fact]
        public void CanSolveUsing3Numbers()
        {
            // Assemble
            var solver = GetSolver();

            // Act
            var sols = solver.GetPossibleSolutions(100, new List<int> { 1, 9, 10 });

            // Assert
            Assert.Equal(0, sols.ClosestDiff);
            Assert.Contains("(9 + 1) × 10 = 100", sols.Solutions);
        }

        [Fact]
        public void CanGetClosestIfNoSolutions()
        {
            // Assemble
            var solver = GetSolver();

            // Act
            var sols = solver.GetPossibleSolutions(10, new List<int> { 4, 5 });

            // Assert
            Assert.Equal(1, sols.ClosestDiff);
            Assert.Contains("5 + 4 = 9", sols.Solutions);
        }
    }
}
