using System.Collections.Generic;
using System.Linq.Expressions;

namespace Countdown.NumbersRound.Expressions
{
    internal class Validator
    {
        private readonly IList<double> _availableNums;
        private int _currIndex;
        private bool _invalid;

        public Validator(IList<double> availableNums)
        {
            _availableNums = availableNums;
        }

        public double CheckExpression(Expression expression)
        {
            _currIndex = 0;

            if (expression.NodeType == ExpressionType.Parameter)
            {
                return _availableNums[_currIndex++];
            }

            _invalid = false;
            return VisitBinary((BinaryExpression)expression);
        }

        private double VisitBinary(BinaryExpression node)
        {
            if (_invalid)
            {
                return double.NaN;
            }

            if (node.Left.NodeType == ExpressionType.Parameter && node.Right.NodeType == ExpressionType.Parameter) // Is a low level node
            {
                double num1 = _availableNums[_currIndex++];
                double num2 = _availableNums[_currIndex++];

                return EvaluateOperation(node.NodeType, num1, num2);
            }
            else if (node.Left.NodeType == ExpressionType.Parameter)
            {
                double num = _availableNums[_currIndex++];
                var rightExp = (BinaryExpression)node.Right;

                return EvaluateOperation(node.NodeType, num, VisitBinary(rightExp));
            }
            else if (node.Right.NodeType == ExpressionType.Parameter)
            {
                double num = _availableNums[_currIndex++];
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

        private double EvaluateOperation(ExpressionType operation, double num1, double num2)
        {
            if (!OperationIsValid(operation, num1, num2))
            {
                _invalid = true;
                return double.NaN;
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
                    return double.NaN;
            }
        }

        private bool OperationIsValid(ExpressionType operation, double num1, double num2)
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
