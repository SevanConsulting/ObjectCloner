using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SC.ObjectCloner
{
    internal static class ExpressionHelpers
    {
        private static MemberExpression GetMemberExpression<TObj, TProp>(this Expression<Func<TObj, TProp>> property)
        {
            var lambda = property as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression unaryExpression)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            return memberExpression;
        }

        /// <summary>
        /// Return the name of the property specified by the expression
        /// </summary>
        /// <typeparam name="TObj">Type of the object on which the property resides</typeparam>
        /// <typeparam name="TProp">Type of the property</typeparam>        
        /// <param name="property">Expression returning a property</param>
        /// <returns>Name of the property of</returns>
        internal static string GetPropertyName<TObj, TProp>(this Expression<Func<TObj, TProp>> property)
        {
            MemberExpression memberExpression = property?.GetMemberExpression();
            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Get a PropertyInfo object for the selected property expression
        /// </summary>
        /// <typeparam name="TObj">Type of the containing object</typeparam>
        /// <typeparam name="TProp">Type of the property referenced by the expression</typeparam>
        /// <param name="propertySelector">A PropertyDescriptor object representing the selected property</param>
        /// <returns></returns>
        internal static PropertyInfo GetPropertyInfo<TObj, TProp>(this Expression<Func<TObj, TProp>> propertySelector)
        {
            return propertySelector?.GetMemberExpression().Member as PropertyInfo;
        }      
    }

    /// <summary>
    /// Get the path of a property expression
    /// </summary>
    /// <remarks> From https://stackoverflow.com/a/22135756/7262 </remarks>
    internal static class PropertyPath
    {
        /// <summary>
        /// Return a dot-separated path for the property expression
        /// </summary>
        /// <typeparam name="TObj">Type of the object the property is declared on</typeparam>
        /// <typeparam name="TProperty">Type of the property to return</typeparam>
        /// <param name="expression">Expression returning the property</param>
        /// <returns></returns>
        public static string GetPath<TObj, TProperty>(this Expression<Func<TObj, TProperty>> expression)
        {
            var visitor = new PropertyVisitor();
            visitor.Visit(expression.Body);
            visitor.Path.Reverse();
            return string.Join(".", visitor.Path.Select(p => p.Name));
        }

        private class PropertyVisitor : ExpressionVisitor
        {
            internal readonly List<PropertyInfo> Path = new List<PropertyInfo>();

            protected override Expression VisitMember(MemberExpression node)
            {
                if (!(node.Member is PropertyInfo))
                {
                    throw new ArgumentException("The path can only contain properties", nameof(node));
                }

                Path.Add((PropertyInfo)node.Member);
                return base.VisitMember(node);
            }
        }
    }
}