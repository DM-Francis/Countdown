using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound.ExpressionBased
{
    internal class Evaluator
    {
        private readonly Stack<float> _numberStack;
        private bool _invalid;

        public Evaluator(IList<float> availableNums)
        {
            _numberStack = new Stack<float>(availableNums);
        }

        public float Evaluate(Expression expression)
        {
            if (expression.GetType() == typeof(ConstantExpression))
            {
                return _numberStack.Pop();
            }

            _invalid = false;
            return VisitBinary((BinaryExpression)expression);
        }

        private float VisitBinary(BinaryExpression node)
        {
            if (_invalid)
            {
                return float.NaN;
            }

            if (node.Left.NodeType == ExpressionType.Constant && node.Right.NodeType == ExpressionType.Constant) // Is a low level node
            {
                float num1 = _numberStack.Pop();
                float num2 = _numberStack.Pop();

                return EvaluateOperation(node.NodeType, num1, num2);
            }
            else if (node.Left.NodeType == ExpressionType.Constant)
            {
                float num = _numberStack.Pop();
                var rightExp = (BinaryExpression)node.Right;

                return EvaluateOperation(node.NodeType, num, VisitBinary(rightExp));
            }
            else if (node.Right.NodeType == ExpressionType.Constant)
            {
                float num = _numberStack.Pop();
                var leftExp = (BinaryExpression)node.Left;

                return EvaluateOperation(node.NodeType, VisitBinary(leftExp), num);
            }
            else
            {
                var leftExp = (BinaryExpression)node.Left;
                var rightExp = (BinaryExpression)node.Right;

                return EvaluateOperation(node.NodeType, VisitBinary(leftExp), VisitBinary(rightExp));
            }
        }

        private float EvaluateOperation(ExpressionType operation, float num1, float num2)
        {
            if (!OperationIsValid(operation, num1, num2))
            {
                _invalid = true;
                return float.NaN;
            }

            switch (operation)
            {
                case ExpressionType.Add:
                    return num1 + num2;
                case ExpressionType.Subtract:
                    return num1 - num2;
                case ExpressionType.Multiply:
                    return num1 * num2;
                case ExpressionType.Divide:
                    return num1 / num2;
                default:
                    _invalid = true;
                    return float.NaN;
            }
        }

        private bool OperationIsValid(ExpressionType operation, float num1, float num2)
        {
            if (num1 < num2)
            {
                return false;
            }

            if (operation == ExpressionType.Multiply && (num1 == 1 || num2 == 1))
            {
                return false;
            }
            else if (operation == ExpressionType.Divide)
            {
                if (num2 == 1)
                {
                    return false;
                }
                else if (num1 % num2 != 0)
                {
                    return false;
                }
                else if (num2 * num2 == num1)
                {
                    return false;
                }
            }
            else if (operation == ExpressionType.Subtract)
            {
                if (num1 == num2)
                {
                    return false;
                }
                else if (num1 == 2 * num2)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
