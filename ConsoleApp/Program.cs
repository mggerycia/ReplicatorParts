using ConsoleApp.Models;
using CORE.Methods;
using CORE.Models;
using IBM.Data.Informix;
using System;
using System.Data;

namespace ConsoleApp
{
    public class Program
    {        
        private static void Main(string[] args)
        {
            var response = Sql.ExecuteReader<Sir_99_Partes>("SELECT TOP 5 * FROM [SIR].[SIR].[SIR_99_PARTES]", new ConnectionMicrosoftSqlServer());
            var dataTable = new DataTable();
            using (var ifxConnection = new IfxConnection(new ConnectionInformix().ConnectionString))
            {
                try
                {
                    ifxConnection.Open();
                    var ifxCommand = new IfxCommand("SELECT * FROM user_web")
                    {
                        Connection = ifxConnection,
                        CommandTimeout = 0
                    };
                                        
                    dataTable.Load(ifxCommand.ExecuteReader());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                finally
                {
                    ifxConnection.Close();
                }
            }

            Console.ReadKey();
        }
    }
}
