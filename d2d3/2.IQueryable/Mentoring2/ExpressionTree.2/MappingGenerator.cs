using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTree._2
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>() where TSource : new() where TDestination : new()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));
            var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(GetInitExpression(sourceParam, typeof(TDestination)), sourceParam);
            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        private Expression GetInitExpression(ParameterExpression parameter, Type targetType)
        {
            var memberBindings = new List<MemberBinding>();
            foreach (var propertyInfo in parameter.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                var targetProperty = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                    .FirstOrDefault(prop => prop.Name == propertyInfo.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (targetProperty.PropertyType.IsArray)
                {
                    memberBindings.Add(GetArrayBinding(targetProperty, propertyInfo, parameter));
                }
                memberBindings.Add(Expression.Bind(targetProperty, Expression.Property(parameter, propertyInfo)));

            }
            return
                Expression.MemberInit(Expression.New(targetType), memberBindings.ToArray());
        }

        private MemberBinding GetArrayBinding(PropertyInfo targetProperty, PropertyInfo sourcePropertyInfo, ParameterExpression parameter)
        {
            // edit method
            Expression<Func<Array, object>> cloneExpression = (b) => b.Clone();
            var arrayExpression = Expression.Convert(Expression.Invoke(cloneExpression, Expression.Property(parameter, sourcePropertyInfo)), targetProperty.PropertyType);
              
            return Expression.Bind(targetProperty, arrayExpression);
        }
    }
}