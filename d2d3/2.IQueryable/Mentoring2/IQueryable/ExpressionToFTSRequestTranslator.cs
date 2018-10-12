using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace IQueryable
{
    public class ExpressionToFTSRequestTranslator : ExpressionVisitor
    {
        private List<StringBuilder> resultStrings;

        public string[] Translate(Expression exp)
        {
            resultStrings = new List<StringBuilder>() { new StringBuilder() };
            Visit(exp);

            return resultStrings.Select(sb => sb.ToString()).ToArray();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(string))
            {
                return HandleStringMethods(node);
            }

            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }
            return base.VisitMethodCall(node);
        }

        private Expression HandleStringMethods(MethodCallExpression node)
        {
            var prop = node.Object as MemberExpression;
            if (prop == null) { throw new ArgumentException("Argument must be a member"); }

            Visit(prop);
            resultStrings[0].Append("(");

            if (node.Method.Name == "EndsWith" || node.Method.Name == "Contains")
            {
                Visit(Expression.Constant("*"));
            }
            Visit(node.Arguments.First());
            if (node.Method.Name == "StartsWith" || node.Method.Name == "Contains")
            {
                Visit(Expression.Constant("*"));
            }
            resultStrings[0].Append(")");
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                    var visitedString = resultStrings[0].ToString();
                    Visit(node.Left);
                    resultStrings.Add(resultStrings[0]);
                    resultStrings[0] = new StringBuilder(visitedString);
                    Visit(node.Right);
                    break;
                case ExpressionType.Equal:
                    if (!HaveMemberAccessAndConstant(node))
                        throw new NotSupportedException(string.Format("Operands must contain constant and member access", node.NodeType));


                    Visit(GetNodeOfType(ExpressionType.MemberAccess, node));
                    resultStrings[0].Append("(");
                    Visit(GetNodeOfType(ExpressionType.Constant, node));
                    resultStrings[0].Append(")");
                    break;

                default:
                    throw new NotSupportedException(string.Format("Operation {0} is not supported", node.NodeType));
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            resultStrings[0].Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            resultStrings[0].Append(node.Value);

            return node;
        }

        private bool HaveMemberAccessAndConstant(BinaryExpression node)
        {
            return node.Left.NodeType == ExpressionType.MemberAccess && node.Right.NodeType == ExpressionType.Constant
                   || node.Left.NodeType == ExpressionType.Constant && node.Right.NodeType == ExpressionType.MemberAccess;

        }

        private Expression GetNodeOfType(ExpressionType expressionType, BinaryExpression node)
        {
            if (node.Left.NodeType == expressionType)
            {
                return node.Left;
            }
            if (node.Right.NodeType == expressionType)
            {
                return node.Right;
            }
            throw new ArgumentException("Invalid expression type");
        }
    }
}
