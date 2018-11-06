using CORE.Models;

namespace ConsoleApp.Models
{
    public class ConnectionMicrosoftSqlServer:Connection
    {
        public ConnectionMicrosoftSqlServer()
        {
            ConnectionString = @"Data Source=10.12.80.36\SQLEXPRESS,1433;database=SIR;User ID=sa;Password=sa_Eryc1@**";
        }
    }
}
