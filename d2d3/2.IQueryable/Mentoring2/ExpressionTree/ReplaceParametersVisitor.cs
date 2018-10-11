using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTree
{
    public class ReplaceParametersVisitor : ExpressionVisitor
    {
        private readonly IDictionary<string, object> _replacementDictionary;

        public ReplaceParametersVisitor(IDictionary<string, object> replacementDictionary)
        {
            _replacementDictionary = replacementDictionary;
        }

        protected override Expression VisitLambda<T>(Expression<T> expression)
        {
            if (!expression.Parameters.Any(p => _replacementDictionary.ContainsKey(p.Name)))
            {
                return base.VisitLambda(expression);
            }

            var resultParameters = expression.Parameters.Except(
                expression.Parameters.Where(p => _replacementDictionary.ContainsKey(p.Name))
            ).ToArray();

            Type returnType;
            if (expression.ReturnType == typeof(void))
            {
                returnType = Expression.GetActionType(resultParameters.Select(p => p.Type).ToArray());
            }
            else
            {
                returnType = Expression.GetFuncType(new[] { expression.ReturnType }
                    .Concat(resultParameters.Select(p => p.Type)).ToArray());
            }
            var body = VisitAndConvert(expression.Body, string.Empty);

            return Expression.Lambda(returnType, body, resultParameters);

        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_replacementDictionary.ContainsKey(node.Name) && _replacementDictionary[node.Name].GetType() == node.Type)
            {
                return Expression.Constant(_replacementDictionary[node.Name]);
            }
            return base.VisitParameter(node);
        }
    }
}