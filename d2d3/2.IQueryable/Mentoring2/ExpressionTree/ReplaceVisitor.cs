using System.Linq.Expressions;

namespace ExpressionTree
{
    public class ReplaceVisitor : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Add)
            {
                if (IsConstantIntAndValue(node.Left, 1))
                {
                    return Expression.Increment(node.Right);
                }
                if (IsConstantIntAndValue(node.Right, 1))
                {
                    return Expression.Increment(node.Left);
                }
            }

            if (node.NodeType == ExpressionType.Subtract)
            {
                if (IsConstantIntAndValue(node.Right, 1))
                {
                    return Expression.Decrement(node.Left);
                }
            }

            return base.VisitBinary(node);
        }

        private bool IsConstantIntAndValue(Expression expression, int expectedValue)
        {
            return expression.NodeType == ExpressionType.Constant &&
                   expression.Type == typeof(int) &&
                   (int) ((ConstantExpression) expression).Value == expectedValue;
        }
    }
}
