using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound
{
    internal class Populator : ExpressionVisitor
    {
        private readonly Stack<float> _availableNums;

        public Populator(IList<float> availableNums)
        {
            _availableNums = new Stack<float>(availableNums);
        }

        public Expression Populate(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Left.NodeType == ExpressionType.Constant && node.Right.NodeType == ExpressionType.Constant) // Is a low level node
            {
                float num1 = _availableNums.Pop();
                float num2 = _availableNums.Pop();

                return Expression.MakeBinary(node.NodeType, Expression.Constant(num1), Expression.Constant(num2));
            }
            else if (node.Left.NodeType == ExpressionType.Constant)
            {
                float num = _availableNums.Pop();
                var newRight = Visit(node.Right);

                return Expression.MakeBinary(node.NodeType, Expression.Constant(num), newRight);
            }
            else if (node.Right.NodeType == ExpressionType.Constant)
            {
                float num = _availableNums.Pop();
                var newLeft = Visit(node.Left);

                return Expression.MakeBinary(node.NodeType, newLeft, Expression.Constant(num));
            }

            return base.VisitBinary(node);
        }
    }
}
