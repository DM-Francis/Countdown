using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound.Expressions
{
    public class StrictExpressionComparer : IEqualityComparer<Expression>
    {
        public bool Equals(Expression x, Expression y)
        {
            if (x.Equals(y)) return true;

            if (x is ConstantExpression consX && y is ConstantExpression consY)
            {
                return consX.Value.Equals(consY.Value);
            }
            else if (x is BinaryExpression binX && y is BinaryExpression binY)
            {
                return binX.NodeType == binY.NodeType && Equals(binX.Left, binY.Left) && Equals(binX.Right, binY.Right);
            }
            else if (x is ParameterExpression parX && y is ParameterExpression parY)
            {
                return parX.Type.Equals(parY.Type);
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(Expression obj)
        {
            if (obj is ConstantExpression cons)
            {
                return cons.Value.GetHashCode();
            }
            else if (obj is BinaryExpression bin)
            {
                return (bin.NodeType.GetHashCode(), GetHashCode(bin.Left), GetHashCode(bin.Right)).GetHashCode();
            }
            else if (obj is ParameterExpression par)
            {
                return par.Type.GetHashCode();
            }
            else
            {
                return obj.GetHashCode();
            }
        }
    }
}
