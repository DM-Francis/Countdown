using Countdown.NumbersRound.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Countdown.NumbersRound.Tests
{
    public class IsomorphicExpressionComparerTests
    {
        private StrictExpressionComparer _strictComparer = new StrictExpressionComparer();

        private IsomorphicExpressionComparer GetComparer()
        {
            return new IsomorphicExpressionComparer();
        }

        [Fact]
        public void CanGetCommutativeExpressionsForSimpleBinary()
        {
            // Assemble
            var comparer = GetComparer();
            var simpleBinary = Expression.MakeBinary(ExpressionType.Add, Expression.Constant(2), Expression.Constant(3)); // 2 + 3

            // Act
            var equivalents = comparer.GetCommutativeEquivalents(simpleBinary).Select(e => e.ToString());

            // Assert
            var expected = new List<string>
            {
                "(2 + 3)",
                "(3 + 2)"
            };

            Assert.Equal(expected, equivalents);            
        }

        [Fact]
        public void CanGetCommutativeExpressionsForNestedBinary()
        {
            // Assemble
            var comparer = GetComparer();
            var baseBinary = Expression.MakeBinary(ExpressionType.Add, Expression.Constant(2), Expression.Constant(3)); // 2 + 3
            var nestedBinary = Expression.MakeBinary(ExpressionType.Add, baseBinary, Expression.Constant(5));           // (2 + 3) + 5

            // Act
            var equivalents = comparer.GetCommutativeEquivalents(nestedBinary).Select(e => e.ToString());

            // Assert
            var expected = new List<string>
            {
                "((2 + 3) + 5)",
                "((3 + 2) + 5)",
                "(5 + (2 + 3))",
                "(5 + (3 + 2))"
            };

            Assert.Equal(expected, equivalents);
        }

        [Fact]
        public void OnlyReturnsSelfForSubractionExpressions()
        {
            // Assemble
            var comparer = GetComparer();
            var subtraction = Expression.MakeBinary(ExpressionType.Subtract, Expression.Constant(5), Expression.Constant(1)); // 5 - 1

            // Act
            var equivalents = comparer.GetCommutativeEquivalents(subtraction).Select(e => e.ToString());

            // Assert
            var expected = new List<string>
            {
                "(5 - 1)"
            };

            Assert.Equal(expected, equivalents);
        }

        [Fact]
        public void CanGetEquivalentsForAdditionNestedInSubtractExpression()
        {
            // Assemble
            var comparer = GetComparer();
            var baseBinary = Expression.MakeBinary(ExpressionType.Add, Expression.Constant(2), Expression.Constant(3)); // 2 + 3
            var nestedBinary = Expression.MakeBinary(ExpressionType.Subtract, baseBinary, Expression.Constant(5));      // (2 + 3) - 5

            // Act
            var equivalents = comparer.GetCommutativeEquivalents(nestedBinary).Select(e => e.ToString());

            // Assert
            var expected = new List<string>
            {
                "((2 + 3) - 5)",
                "((3 + 2) - 5)"
            };

            Assert.Equal(expected, equivalents);
        }
    }
}
