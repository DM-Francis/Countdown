using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound
{
    internal enum PopulateMode
    {
        Index,
        Constant
    }

    internal class Populator : ExpressionVisitor
    {
        private readonly ParameterExpression _arrayParameter;
        private readonly List<double> _availableNums;
        private int _currIndex = 0;
        private PopulateMode _mode;

        public Populator(ParameterExpression arrayParameter)
        {
            _arrayParameter = arrayParameter;
            _mode = PopulateMode.Index;
        }

        public Populator(List<double> availableNums)
        {
            _availableNums = availableNums;
            _mode = PopulateMode.Constant;
        }

        public Expression Populate(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Parameter)
            {
                return CreateEndNodeExpression();
            }

            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.ArrayIndex)
            {
                return base.VisitBinary(node);
            }

            if (node.Left.NodeType == ExpressionType.Parameter && node.Right.NodeType == ExpressionType.Parameter) // Is a low level node
            {
                var param1 = CreateEndNodeExpression();
                var param2 = CreateEndNodeExpression();

                return Expression.MakeBinary(node.NodeType, param1, param2);
            }
            else if (node.Left.NodeType == ExpressionType.Parameter)
            {
                var param = CreateEndNodeExpression();
                var newRight = Visit(node.Right);

                return Expression.MakeBinary(node.NodeType, param, newRight);
            }
            else if (node.Right.NodeType == ExpressionType.Parameter)
            {
                var param = CreateEndNodeExpression();
                var newLeft = Visit(node.Left);

                return Expression.MakeBinary(node.NodeType, newLeft, param);
            }

            return base.VisitBinary(node);
        }

        private Expression CreateEndNodeExpression()
        {
            if (_mode == PopulateMode.Index)
            {
                return Expression.ArrayIndex(_arrayParameter, Expression.Constant(_currIndex++, typeof(int)));
            }
            else if (_mode == PopulateMode.Constant)
            {
                return Expression.Constant(_availableNums[_currIndex++], typeof(double));
            }

            throw new InvalidOperationException();
        }
    }
}
