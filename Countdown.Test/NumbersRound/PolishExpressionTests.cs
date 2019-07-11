using Countdown.NumbersRound.PolishNotationBased;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Countdown.Test.NumbersRound
{
    public class PolishExpressionTests
    {
        [Fact]
        public void ThrowsOnInvalidInputExpression()
        {
            // Arrange
            var input = new List<string> { "1", "2", "3" }; // Invalid -> must start with an operation +,-,/,*

            // Act & Assert
            Assert.Throws<FormatException>(() => new PolishExpression(input));
        }

        [Fact]
        public void CanEvaluateOnePlusOne()
        {
            // Arrange
            var input = new List<string> { "+", "1", "1" };
            var exp = new PolishExpression(input);

            // Act
            var result = exp.Evaluate();

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void CanEvaluateMoreComplexExpression()
        {
            // Arrange
            var input = new List<string> { "*", "+", "1", "2", "3" }; // (1 + 2) * 3
            var exp = new PolishExpression(input);

            // Act 
            var result = exp.Evaluate();

            // Assert
            Assert.Equal(9, result);
        }
    }
}
