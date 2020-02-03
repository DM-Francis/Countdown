using Countdown.NumbersRound.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Countdown.NumbersRound.Tests
{
    public class StrictExpressionComparerTests
    {
        private StrictExpressionComparer GetComparer()
        {
            return new StrictExpressionComparer();
        }

        [Fact]
        public void CompareTwoEqualConstantExpressions()
        {
            // Assemble
            var comparer = GetComparer();

            // Act
            var result = comparer.Equals(Expression.Constant(5), Expression.Constant(5));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CompareTwoDifferentConstantExpressions()
        {
            // Assemble
            var comparer = GetComparer();

            // Act
            var result = comparer.Equals(Expression.Constant(1), Expression.Constant(5));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CompareTwoEqualBinaryExpressions()
        {
            // Assemble
            var comparer = GetComparer();
            var binA = Expression.MakeBinary(ExpressionType.Add, Expression.Constant(1), Expression.Constant(2));
            var binB = Expression.MakeBinary(ExpressionType.Add, Expression.Constant(1), Expression.Constant(2));

            // Act
            var result = comparer.Equals(binA, binB);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CompareTwoDifferentBinaryExpressions()
        {
            // Assemble
            var comparer = GetComparer();
            var binA = Expression.MakeBinary(ExpressionType.Add, Expression.Constant(1), Expression.Constant(2));
            var binB = Expression.MakeBinary(ExpressionType.Subtract, Expression.Constant(1), Expression.Constant(2));

            // Act
            var result = comparer.Equals(binA, binB);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CompareParameterExpressions()
        {
            // Assemble
            var comparer = GetComparer();
            var parA = Expression.Parameter(typeof(int), "a");
            var parB = Expression.Parameter(typeof(int), "b");

            // Act
            var result = comparer.Equals(parA, parB);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CompareDifferentParameterExpressions()
        {
            // Assemble
            var comparer = GetComparer();
            var parA = Expression.Parameter(typeof(int), "a");
            var parB = Expression.Parameter(typeof(string), "b");

            // Act
            var result = comparer.Equals(parA, parB);

            // Assert
            Assert.False(result);
        }
    }
}
