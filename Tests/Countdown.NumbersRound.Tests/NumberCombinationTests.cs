using Countdown.NumbersRound.Solve;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Countdown.NumbersRound.Tests
{
    public class NumberCombinationTests
    {
        [Fact]
        public void EqualsIsCorrectForSameList()
        {
            // Assemble
            var numbers = new List<double> { 1, 2, 3, 4, 5 };
            var comb = new NumberCombination(numbers);

            // Act
            bool equal = comb.Equals(comb);

            // Assert
            Assert.True(equal);
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.True(comb == comb);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.Equal(comb.GetHashCode(), comb.GetHashCode());
        }

        [Fact]
        public void EqualsIsCorrectForEquivalentList()
        {
            // Assemble
            var numbers1 = new List<double> { 1, 2, 3, 4, 5 };
            var numbers2 = new List<double> { 1, 2, 3, 4, 5 };
            var comb1 = new NumberCombination(numbers1);
            var comb2 = new NumberCombination(numbers2);

            // Act
            bool equal = comb1.Equals(comb2);

            // Assert
            Assert.True(equal);
            Assert.True(comb1 == comb2);
            Assert.Equal(comb1.GetHashCode(), comb2.GetHashCode());
        }

        [Fact]
        public void EqualsIsCorrectForEquivalentListsWithDupes()
        {
            // Assemble
            var numbers1 = new List<double> { 1, 2, 3, 3, 4, 5 };
            var numbers2 = new List<double> { 1, 2, 3, 3, 4, 5 };
            var comb1 = new NumberCombination(numbers1);
            var comb2 = new NumberCombination(numbers2);

            // Act
            bool equal = comb1.Equals(comb2);

            // Assert
            Assert.True(equal);
            Assert.True(comb1 == comb2);
            Assert.Equal(comb1.GetHashCode(), comb2.GetHashCode());
        }

        [Fact]
        public void EqualsIsCorrectForEquivalentListsOutOfOrder()
        {
            // Assemble
            var numbers1 = new List<double> { 1, 2, 3, 3, 4, 5 };
            var numbers2 = new List<double> { 5, 4, 3, 2, 3, 1 };
            var comb1 = new NumberCombination(numbers1);
            var comb2 = new NumberCombination(numbers2);

            // Act
            bool equal = comb1.Equals(comb2);

            // Assert
            Assert.True(equal);
            Assert.True(comb1 == comb2);
            Assert.Equal(comb1.GetHashCode(), comb2.GetHashCode());
        }

        [Fact]
        public void EqualsIsFalseForDifferentSizedLists()
        {
            // Assemble
            var numbers1 = new List<double> { 1, 2, 3, 4, 5 };
            var numbers2 = new List<double> { 1, 2, 3, 4 };
            var comb1 = new NumberCombination(numbers1);
            var comb2 = new NumberCombination(numbers2);

            // Act
            bool equal = comb1.Equals(comb2);

            // Assert
            Assert.False(equal);
            Assert.False(comb1 == comb2);
            Assert.NotEqual(comb1.GetHashCode(), comb2.GetHashCode());
        }

        [Fact]
        public void EqualsIsFalseForDifferentLists()
        {
            // Assemble
            var numbers1 = new List<double> { 1, 2, 3, 4, 5 };
            var numbers2 = new List<double> { 1, 2, 3, 4, 6 };
            var comb1 = new NumberCombination(numbers1);
            var comb2 = new NumberCombination(numbers2);

            // Act
            bool equal = comb1.Equals(comb2);

            // Assert
            Assert.False(equal);
            Assert.False(comb1 == comb2);
            Assert.NotEqual(comb1.GetHashCode(), comb2.GetHashCode());
        }
    }
}
