using System.Collections.Generic;
using System.Linq;

namespace CORE.Extensions
{
    public static class ListExtensions
    {
        public static string ToStringByApostrophes(this List<string> strings)
        {
            return strings.Aggregate("", (current, s) => current + $"'{s}', ")
                .Replace("'',", "")
                .TrimEnd(' ', ',');
        }

	    public static string ToStringByCommas(this List<string> strings)
	    {
		    return strings.Aggregate("", (current, @string) => current + $"{@string}, ").TrimEnd(' ', ',');
	    }

	    public static string ToStringKeyValueByCommas(this List<List<string>> keysValues)
	    {
		    return keysValues.Aggregate("", (current, t) => current + $"{t[0]} = {t[1]}, ").TrimEnd(',', ' ');
	    }

	    public static string ToStringKeyValueByAnd(this List<List<string>> keysValues)
	    {
		    return keysValues.Aggregate("", (current, t) => current + $"{t[0]} = {t[1]} AND ").TrimEnd('A', 'N', 'D', ' ');
	    }
	}
}
