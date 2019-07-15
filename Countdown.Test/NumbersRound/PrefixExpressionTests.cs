using Countdown.NumbersRound.PrefixNotationBased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Countdown.Test.NumbersRound
{
    public class PrefixExpressionTests
    {
        [Fact]
        public void ExpressionMustStartWithAnOperation()
        {
            // Arrange
            var input = new List<string> { "1", "2", "3" }; // Invalid -> must start with an operation +,-,/,*

            // Act & Assert
            Assert.Throws<FormatException>(() => new PrefixExpression(input));
        }

        [Fact]
        public void CanEvaluateOnePlusOne()
        {
            // Arrange
            var input = new List<string> { "+", "1", "1" };
            var exp = new PrefixExpression(input);

            // Act
            var result = exp.Evaluate();

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void CanEvaluateComplexExpression1()
        {
            // Arrange
            var input = new List<string> { "*", "+", "1", "2", "3" }; // (1 + 2) * 3
            var exp = new PrefixExpression(input);

            // Act 
            var result = exp.Evaluate();

            // Assert
            Assert.Equal(9, result);
        }

        [Fact]
        public void CanEvaluateComplexExpression2()
        {
            // Arrange
            var input = new List<string> { "/", "2", "-", "+", "10", "3.5", "9.5" }; //   2 / ((10 + 3.5) - 9.5)
            var exp = new PrefixExpression(input);

            // Act
            var result = exp.Evaluate();

            // Assert
            Assert.Equal(0.5, result);
        }

        [Fact]
        public void ThrowsWhenEvaluatingExpressionWithInvalidFormat()
        {
            // Arrange
            var input = new List<string> { "-", "3", "4", "+", "4", "7", "1" };
            var exp = new PrefixExpression(input);

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => exp.Evaluate());
        }

        [Fact]
        public void ExpressionMustHaveAnOddNumberOfElements()   // n numbers + n-1 operations = 2n-1 which is always odd
        {
            // Arrange
            var input = new List<string> { "+", "5" };

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => new PrefixExpression(input));
        }

        [Fact]
        public void ExpressionMustFinishWithANumber()
        {
            // Arrange
            var input = new List<string> { "-", "3", "4", "5", "+" };

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => new PrefixExpression(input));
        }

        [Fact]
        public void ConstructedWithSingleNumberEvaluatesToNumber()
        {
            // Arrange
            var exp = new PrefixExpression("1");

            // Act
            var result = exp.Evaluate();

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void CanEvaluateComplexExpression3()
        {
            // Arrange
            var input = new List<string> { "+", "+", "1", "+", "1", "1", "1" }; //   (1 + (1 + 1)) + 1
            var exp = new PrefixExpression(input);

            // Act
            var result = exp.Evaluate();

            // Assert
            Assert.Equal(4, result);
        }
    }
}