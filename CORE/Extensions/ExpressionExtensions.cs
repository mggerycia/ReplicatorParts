using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CORE.Extensions
{
	public static class ExpressionExtensions
	{
		public static List<string> GetArguments<T>(this Expression<Func<T, object>> expression)
		{
			return ((NewExpression)expression.Body).Arguments
				.Select(argument => ((MemberExpression)argument).Member.Name).ToList().Distinct().ToList();
		}
	}
}
