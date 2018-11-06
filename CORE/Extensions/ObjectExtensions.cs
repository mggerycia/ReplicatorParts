using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CORE.Extensions
{
	public static class ObjectExtensions
	{
		public static List<string[]> GetProperties<T>(this T @object, Expression<Func<T, object>> expression = null, bool isExcluded = true) where T : class
		{
			var properties = new List<string>();
			if (expression != null)
				properties = expression.GetArguments();

			var propertiesWithValue = new List<string[]>();
			foreach (var propertyCurrent in typeof(T).GetProperties())
			{
				var property = properties.SingleOrDefault(p => p.Equals(propertyCurrent?.Name ?? ""));
				if (property != null)
				{
					if (!isExcluded)
						propertiesWithValue.Add(new[]
							{propertyCurrent?.Name ?? "", propertyCurrent?.GetValue(@object, null)?.ToString() ?? ""});
				}
				else
					propertiesWithValue.Add(new[]
						{propertyCurrent?.Name ?? "", propertyCurrent?.GetValue(@object, null)?.ToString() ?? ""});
			}

			return propertiesWithValue;
		}

		public static string ToInsert<T>(this T @object, Expression<Func<T, object>> expression = null, bool isExcluded = true) where T : class
		{
			var columnsValues = @object.GetNamesValues(expression != null ? expression.GetArguments() : new List<string>(), isExcluded);
			return $"INSERT INTO {typeof(T).Name} ({columnsValues[0].ToStringByCommas()}) VALUES ({columnsValues[1].ToStringByCommas()}) ";
		}

		public static string ToUpdate<T>(this T @object, Expression<Func<T, object>> expression = null, bool isExcluded = true, Func<object> expressionWhere = null) where T : class
		{
			return $"UPDATE {typeof(T).Name} SET {@object.GetKeyValue(expression != null ? expression.GetArguments() : new List<string>(), isExcluded).ToStringKeyValueByCommas()} {(expressionWhere != null ? $" WHERE {expressionWhere.GetKeyValue().ToStringKeyValueByAnd()}" : "")}";
		}
		
		public static string ToDelete(this string @string, Func<object> expressionWhere = null)
		{
			return $"DELETE FROM {@string} {(expressionWhere != null ? $" WHERE {expressionWhere.GetKeyValue().ToStringKeyValueByAnd()}" : "")}";
		}

		private static List<List<string>> GetNamesValues<T>(this T @object, List<string> properties, bool isEscluded = true)
		{
			var columns = new List<string>();
			var values = new List<string>();

			foreach (var propertyCurrent in typeof(T).GetProperties())
			{
				var property = properties.SingleOrDefault(p => p.Equals(propertyCurrent?.Name ?? ""));
				var flag = isEscluded ? property == null : property != null;
				if (!flag) continue;

				columns.Add(propertyCurrent.Name);
				values.Add(GetValueSqlDbType(@object, propertyCurrent));
			}

			return new List<List<string>> { columns, values };
		}

		private static List<List<string>> GetKeyValue<T>(this T @object, List<string> properties, bool isEscluded = true)
		{
			return (from propertyCurrent in typeof(T).GetProperties()
				let property = properties.SingleOrDefault(p => p.Equals(propertyCurrent?.Name ?? ""))
				let flag = isEscluded ? property == null : property != null
				where flag
				select new List<string>() {propertyCurrent.Name, GetValueSqlDbType(@object, propertyCurrent)}).ToList();
		}

		public static List<List<string>> GetKeyValue(this Func<object> expression)
		{
			var expressionList = expression()
				.GetType()
				.GetProperties()
				.Select(f => new { Name = f.Name, Value = f.GetValue(expression(), null) });

			return expressionList.Select(item => new List<string>()
				{
					item.Name,
					GetValueSqlDbType<object>(null, null, item.Value)
				})
				.ToList();
		}

		private static string GetValueSqlDbType<T>(T @object, PropertyInfo property, object val = null)
		{
			string value;
			var parameter = new SqlCommand().CreateParameter();
			parameter.Value = @object != null ? property.GetValue(@object, null) ?? DBNull.Value : val ?? DBNull.Value;

			switch (parameter.SqlDbType)
			{
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Text:
				case SqlDbType.VarChar:
					value = (parameter.Value.Equals(DBNull.Value)
						? "NULL"
						: "'" + parameter.Value + "'");
					break;
				case SqlDbType.BigInt:
				case SqlDbType.Decimal:
				case SqlDbType.Int:
				case SqlDbType.Money:
				case SqlDbType.SmallInt:
				case SqlDbType.SmallMoney:
				case SqlDbType.TinyInt:
				case SqlDbType.Float:
				case SqlDbType.Real:
					value = (parameter.Value.Equals(DBNull.Value)
						? "NULL"
						: parameter.Value.ToString());
					break;
				case SqlDbType.Date:
				case SqlDbType.DateTime:
				case SqlDbType.DateTime2:
				case SqlDbType.DateTimeOffset:
					value = (parameter.Value.Equals(DBNull.Value)
						? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
						: "'" + DateTime.Parse(parameter.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "'");
					break;
				case SqlDbType.Bit:
					value = (parameter.Value.Equals(DBNull.Value)
						? "NULL"
						: (parameter.Value.Equals(false) ? "0" : "1"));
					break;
				default:
					value = (parameter.Value.Equals(DBNull.Value)
						? "NULL"
						: parameter.Value.ToString());
					break;
			}

			return value;
		}
	}
}
