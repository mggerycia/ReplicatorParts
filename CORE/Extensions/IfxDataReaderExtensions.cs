using CORE.Extensions;
using IBM.Data.Informix;
using System;
using System.Collections.Generic;

namespace CORE.Extensions
{
    public static class IfxDataReaderExtensions
    {
        public static List<T> Binding<T>(this IfxDataReader sqlDataReader) where T : class
        {
            var objects = new List<T>();
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

            return objects;
        }

        public static bool HasColumn(this IfxDataReader reader, string columnName)
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
