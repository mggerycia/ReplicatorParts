using System;
using System.Linq.Expressions;

namespace CORE.Extensions
{
    public static class NameOfExtensions
    {
        public static string NameOf<T, TT>(this Expression<Func<T, TT>> accessor)
        {
            return NameOf(accessor.Body);
        }

        public static string NameOf<T>(this Expression<Func<T>> accessor)
        {
            return NameOf(accessor.Body);
        }

        public static string NameOf<T, TT>(this T obj, Expression<Func<T, TT>> propertyAccessor)
        {
            return NameOf(propertyAccessor.Body);
        }

        private static string NameOf(Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                if (!(expression is MemberExpression memberExpression))
                    return null;
                return memberExpression.Member.Name;
            }
            return null;
        }
    }
}
