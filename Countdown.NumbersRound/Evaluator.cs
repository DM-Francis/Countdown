using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound
{
    internal class Evaluator
    {
        private readonly Stack<double> _numberStack;
        private bool _invalid;

        public Evaluator(IList<double> availableNums)
        {
            _numberStack = new Stack<double>(availableNums);
        }

        public double Evaluate(Expression expression)
        {
            if (expression.GetType() == typeof(ConstantExpression))
            {
                return _numberStack.Pop();
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

            if (node.Left.NodeType == ExpressionType.Constant && node.Right.NodeType == ExpressionType.Constant) // Is a low level node
            {
                double num1 = _numberStack.Pop();
                double num2 = _numberStack.Pop();

                return EvaluateOperation(node.NodeType, num1, num2);
            }
            else if (node.Left.NodeType == ExpressionType.Constant)
            {
                double num = _numberStack.Pop();
                var rightExp = (BinaryExpression)node.Right;

                return EvaluateOperation(node.NodeType, num, VisitBinary(rightExp));
            }
            else if (node.Right.NodeType == ExpressionType.Constant)
            {
                double num = _numberStack.Pop();
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
