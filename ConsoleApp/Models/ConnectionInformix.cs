using CORE.Models;

namespace ConsoleApp.Models
{
    public class ConnectionInformix:Connection
    {
        public ConnectionInformix()
        {
            ConnectionString = @"Database=saai;Host=10.12.80.20;Server=eryciadb;Service=62000;Protocol=onsoctcp;UID=gmanuel;Password=magar*#;";
        }               
    }
}
