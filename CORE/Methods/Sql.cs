using System;
using System.Collections.Generic;
using System.Data;
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
                    if (!call) return new Response(Status.Exception, exception.Message);
                    ExecuteNonQuery(query, connection, false);

                    return new Response(Status.Exception, exception.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public static Response ExecuteNonStoredProcedure(string storedProcedure, Connection connection, bool call = true)
        {
            using(var sqlConnection=new SqlConnection(connection.ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand(storedProcedure)
                    {
                        Connection = sqlConnection,
                        CommandTimeout = 0//,
                        //CommandType = CommandType.StoredProcedure
                    };

                    //var response = ;

                    // - Falta agregar parametros
                    // - Falta ver como recibir la respuesta al ejecutar
                    //      ya sea crear una clase para este tipo de respuestas 
                    //      o devolver el valor solamente dentro del Data del Response
                    var dataTable = new DataTable();
                    dataTable.Load(sqlCommand.ExecuteReader());

                    return new Response(dataTable);
                }
                catch (Exception exception)
                {
                    if (!call) return new Response(Status.Exception, exception.Message);
                    ExecuteNonStoredProcedure(storedProcedure, connection, false);

                    return new Response(Status.Exception, exception.Message);
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
                    if (!call) return new Response(Status.Exception, exception.Message);
                    ExecuteReader<T>(query, connection, false);

                    return new Response(Status.Exception, exception.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public static Response ExecuteReader(string query, Connection connection, bool call = true) 
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

                    var dataTable = new DataTable();
                    dataTable.Load(sqlCommand.ExecuteReader());

                    return new Response(dataTable);
                }
                catch (Exception exception)
                {
                    if (!call) return new Response(Status.Exception, exception.Message);
                    ExecuteReader(query, connection, false);

                    return new Response(Status.Exception, exception.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
    }
}
