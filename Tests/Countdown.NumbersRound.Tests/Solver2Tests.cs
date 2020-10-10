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
        [Fact]
        public void InitialTesting()
        {
            // Assemble
            var availableNums = new List<int>
            {
                25, 50, 75, 100, 1, 3
            };

            var solver = new Solver2(561, availableNums);

            // Act
            solver.Solve();
        }

        [Fact]
        public void InitialTesting2()
        {
            // Assemble
            var availableNums = new List<int>
            {
                1,2,3,4,5,6
            };

            var solver = new Solver2(100, availableNums);

            // Act
            solver.Solve();
        }

        [Fact]
        public void InitialTesting3()
        {
            // Assemble
            var availableNums = new List<int>
            {
                5,2,1,7,7,8
            };

            var solver = new Solver2(903, availableNums);

            // Act
            solver.Solve();
        }
    }
}
