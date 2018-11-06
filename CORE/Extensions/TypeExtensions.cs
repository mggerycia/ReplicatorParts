using System;
using System.Reflection;

namespace CORE.Extensions
{
    public static class TypeExtensions
    {
        public static object GetDefault(this Type T)
        {
            if (T == null)
                throw new NullReferenceException();

            var method = typeof(TypeExtensions).GetMethod("_GetDefault", BindingFlags.Static | BindingFlags.NonPublic);

            var generic = method.MakeGenericMethod(T);

            return generic.Invoke(null, new object[0]);
        }

        private static T _GetDefault<T>()
        {
            return default(T);
        }
    }
}
