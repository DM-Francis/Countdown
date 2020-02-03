using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound.Expressions
{
    public class IsomorphicExpressionComparer : IEqualityComparer<Expression>
    {
        private readonly StrictExpressionComparer _strict = new StrictExpressionComparer();

        public bool Equals(Expression x, Expression y)
        {
            if (_strict.Equals(x, y))
            {
                return true;
            }

            if (x is BinaryExpression binX && y is BinaryExpression binY)
            {
                if (!CommutativeEquals(binX, binY)) return false;


            }

            return false;
        }

        private bool CommutativeEquals(BinaryExpression x, BinaryExpression y)
        {
            var equivalents = GetCommutativeEquivalents(x);

            return equivalents.Any(e => _strict.Equals(e, y));
        }

        private bool AssociativeEquals(BinaryExpression x, BinaryExpression y)
        {
            if (x.NodeType != y.NodeType) return false;
            if (!(x.NodeType == ExpressionType.Add || x.NodeType == ExpressionType.Multiply)) return false;

            // Must both have a binaryexpression subnode with the same operation in order to compare
            if (!HasSubnodeWithSameOperation(x)) return false;
            if (!HasSubnodeWithSameOperation(y)) return false;

            if (BothSubnodesHaveSameOperation(x))
            {
                // Example: (a + b) + (c + d)
            }

            return false;
        }

        public IEnumerable<Expression> GetCommutativeEquivalents(Expression exp)
        {
            if (exp is BinaryExpression binary)
            {
                var possibleLefts = GetCommutativeEquivalents(binary.Left);
                var possibleRights = GetCommutativeEquivalents(binary.Right);

                var allStandards =  from left in possibleLefts
                                    from right in possibleRights
                                    select Expression.MakeBinary(binary.NodeType, left, right);

                if (exp.NodeType == ExpressionType.Add || exp.NodeType == ExpressionType.Multiply)
                {
                    var allSwitched = from left in possibleLefts
                                      from right in possibleRights
                                      select Expression.MakeBinary(binary.NodeType, right, left);

                    return allStandards.Union(allSwitched);
                }
                else
                {
                    return allStandards;
                }
            }
            else
            {
                return new[] { exp };
            }
        }

        public IEnumerable<BinaryExpression> GetAssociativeEquivalents(BinaryExpression exp)
        {
            if (exp.Left is BinaryExpression binLeft && exp.Left.NodeType == exp.NodeType)
            {
                var possibleLefts = GetAssociativeEquivalents(binLeft);

                foreach(var possibleLeft in possibleLefts)
                {
                    yield return Expression.MakeBinary(exp.NodeType, possibleLeft, exp.Right);
                    yield return Expression.MakeBinary(exp.NodeType, possibleLeft.Left, Expression.MakeBinary(exp.NodeType, possibleLeft.Right, exp.Right));
                }
            }
        }

        private static bool HasSubnodeWithSameOperation(BinaryExpression x)
        {
            return x.Left.NodeType == x.NodeType || x.Right.NodeType == x.NodeType;
        }

        private static bool BothSubnodesHaveSameOperation(BinaryExpression x)
        {
            return x.Left.NodeType == x.Right.NodeType && x.Left.NodeType == x.NodeType;
        }

        public int GetHashCode(Expression obj)
        {
            throw new NotImplementedException();
        }
    }
}
