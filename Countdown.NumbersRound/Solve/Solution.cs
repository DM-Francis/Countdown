using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound.Solve
{
    internal class Solution
    {
        public IReadOnlyList<Operation> Operations { get; }
        public int Result => (int)Operations.Last().Result;
        public IEnumerable<int> NumbersUsed => GetNumbersUsed(AsExpression());

        public Solution(IList<Operation> operations)
        {
            var internalList = new List<Operation>(operations);
            _ = AsExpression(internalList, out List<Operation> unneededOperations);
            foreach (var operation in unneededOperations)
            {
                internalList.Remove(operation);
            }

            Operations = new ReadOnlyCollection<Operation>(internalList);
        }

        public BinaryExpression AsExpression() => AsExpression(Operations, out _);

        private static BinaryExpression AsExpression(IEnumerable<Operation> operations, out List<Operation> unneededOperations)
        {
            var remainingOperations = new List<Operation>(operations);
            var lastOperation = remainingOperations.Last();
            remainingOperations.Remove(lastOperation);

            var finalExpression = CreateExpressionTreeFromOperation(lastOperation, remainingOperations);
            unneededOperations = remainingOperations;

            return finalExpression;
        }

        private static BinaryExpression CreateExpressionTreeFromOperation(Operation operation, List<Operation> remainingOperations)
        {
            var leftExp = CreateExpressionFromResult(operation.A, remainingOperations);
            var rightExp = CreateExpressionFromResult(operation.B, remainingOperations);

            return Expression.MakeBinary(operation.Operator.Type, leftExp, rightExp);
        }

        private static Expression CreateExpressionFromResult(double result, List<Operation> remainingOperations)
        {
            Operation? opToCreateResult = remainingOperations.Find(op => op.Result == result);
            if (opToCreateResult is null)
            {
                return Expression.Constant(result);
            }
            else
            {
                remainingOperations.Remove(opToCreateResult);
                return CreateExpressionTreeFromOperation(opToCreateResult, remainingOperations);
            }
        }

        public override string ToString()
        {
            BinaryExpression expression = AsExpression();
            string rawExpression = expression.ToString();
            string expWithoutTrailingBrackets = rawExpression.Substring(1, rawExpression.Length - 2);

            return $"{expWithoutTrailingBrackets} = {Result}";
        }

        private static IEnumerable<int> GetNumbersUsed(Expression expression)
        {
            if (expression is ConstantExpression constant)
            {
                yield return (int)(double)constant.Value;
            }
            else if (expression is BinaryExpression binary)
            {
                foreach (var num in GetNumbersUsed(binary.Left))
                    yield return num;
                foreach (var num in GetNumbersUsed(binary.Right))
                    yield return num;
            }
        }
    }
}
