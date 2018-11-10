using CORE.Extensions;
using CORE.Models;
using IBM.Data.Informix;
using System;

namespace CORE.Methods
{
    public class Informix
    {
        public static Response ExecuteNonQuery(string query, Connection connection, bool call = true)
        {
            using (var ifxConnection = new IfxConnection(connection.ConnectionString))
            {
                try
                {
                    ifxConnection.Open();
                    var ifxCommand = new IfxCommand(query)
                    {
                        Connection = ifxConnection,
                        CommandTimeout = 0
                    };

                    return new Response(ifxCommand.ExecuteNonQuery());
                }
                catch (Exception exception)
                {
                    if (!call) return new Response(Status.Exception, exception.Message);
                    ExecuteNonQuery(query, connection, false);

                    return new Response(Status.Exception, exception.Message);
                }
                finally
                {
                    ifxConnection.Close();
                }
            }
        }

        public static Response ExecuteReader<T>(string query, Connection connection, bool call = true) where T : class
        {
            using (var ifxConnection = new IfxConnection(connection.ConnectionString))
            {
                try
                {
                    ifxConnection.Open();
                    var ifxCommand = new IfxCommand(query)
                    {
                        Connection = ifxConnection,
                        CommandTimeout = 0
                    };

                    return new Response(ifxCommand.ExecuteReader().Binding<T>());
                }
                catch (Exception exception)
                {
                    if (!call) return new Response(Status.Exception, exception.Message);
                    ExecuteReader<T>(query, connection, false);

                    return new Response(Status.Exception, exception.Message);
                }
                finally
                {
                    ifxConnection.Close();
                }
            }
        }
    }
}
