using System;
using System.Collections.Generic;
using System.Text;

namespace Countdown.NumbersRound.Expressions
{
    internal class Operation : ExpressionBase
    {
        public OperationType Type { get; }

        public override float Value
        {
            get
            {
                var operation = (Operation)Data;
                return PerformOperation(Left.Value, Right.Value, operation.Type);
            }
        }

        public static float PerformOperation(float left, float right, OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Subtraction:
                    if (left <= right)
                    {
                        return float.NaN;
                    }
                    return left - right;
                case OperationType.Multiplication:
                    return left * right;
                case OperationType.Division:
                    if (right == 0)
                    {
                        return float.NaN;
                    }
                    var output = left / right;

                    if (output % 1 != 0)
                    {
                        return float.NaN;
                    }

                    return output;
                default:
                    return left + right;
            }
        }
    }
}
