using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CORE.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static List<T> Binding<T>(this SqlDataReader sqlDataReader) where T : class
        {
            var objects=new List<T>();
            try
            {
                while (sqlDataReader.Read())
                {
                    var entity = (T)Activator.CreateInstance(typeof(T));
                    foreach (var property in typeof(T).GetProperties())
                    {
                        var value = property.PropertyType.GetDefault();

                        if (!sqlDataReader.HasColumn(property.Name))
                        {
                            property.SetValue(entity, value, null);
                            continue;
                        }

                        var index = sqlDataReader.GetOrdinal(property.Name);

                        if (!sqlDataReader.IsDBNull(index))
                        {
                            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            value = Convert.ChangeType(sqlDataReader.GetValue(index), type);
                        }

                        property.SetValue(entity, value, null);
                    }
                    objects.Add(entity);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return objects.Count > 0 ? objects : null;
        }

        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
