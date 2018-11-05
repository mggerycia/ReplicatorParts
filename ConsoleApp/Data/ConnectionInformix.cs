using IBM.Data.Informix;
using System;
using System.Data;

namespace ConsoleApp.Data
{
    public class ConnectionInformix
    {
        public ConnectionInformix() { }

        public string Test()
        {
            var ifxConnection = new IfxConnection
            {
                ConnectionString = $"Database=saai;Host=10.12.80.20;Server=eryciadb;Service=62000;Protocol=onsoctcp;UID=gmanuel;Password=magar*#;"
            };
            try
            {            
                ifxConnection.Open();
                return "Successful Connection!";
            }
            catch (IfxException ifxException)
            {
                return $"Error Connection \nMessage: { ifxException.ToString()}";
            }
            finally
            {
                ifxConnection.Close();
            }
        }

        public DataTable GetData(string query)
        {
            var ifxConnection = new IfxConnection
            {
                ConnectionString = $"Database=saai;Host=10.12.80.20;Server=eryciadb;Service=62000;Protocol=onsoctcp;UID=gmanuel;Password=magar*#;"
            };
            try
            {                
                var ifxCommand = new IfxCommand(query, ifxConnection);
                ifxCommand.Connection.Open();
                var dataTable = new DataTable();
                dataTable.Load(ifxCommand.ExecuteReader());

                return dataTable;                
            }
            catch (IfxException ifxException)
            {
                return null;                
            }
            finally
            {
                ifxConnection.Close();
            }
        }
    }
}
