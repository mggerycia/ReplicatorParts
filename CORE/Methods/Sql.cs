using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CORE.Extensions;
using CORE.Models;

namespace CORE.Methods
{
    public class Sql
    {
        public static Response ExecuteNonQuery(string query, Connection connection, bool call = true)
        {
            using (var sqlConnection = new SqlConnection(connection.ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand(query)
                    {
                        Connection = sqlConnection,
                        CommandTimeout = 0
                    };

                    return new Response(sqlCommand.ExecuteNonQuery());
                }
                catch (Exception exception)
                {
                    if (!call) return new Response(exception.Message);
                    ExecuteNonQuery(query, connection, false);

                    return new Response(exception.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public static Response ExecuteReader<T>(string query, Connection connection, bool call = true) where T : class
        {
            using (var sqlConnection = new SqlConnection(connection.ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand(query)
                    {
                        Connection = sqlConnection,
                        CommandTimeout = 0
                    };

                    return new Response(sqlCommand.ExecuteReader().Binding<T>());
                }
                catch (Exception exception)
                {
                    if (!call) return new Response(exception.Message);
                    ExecuteReader<T>(query, connection, false);

                    return new Response(exception.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
    }
}
