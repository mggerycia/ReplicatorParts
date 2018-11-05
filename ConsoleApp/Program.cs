using ConsoleApp.Data;
using System;

namespace ConsoleApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var connectionInformix = new ConnectionInformix();
            Console.WriteLine(connectionInformix.Test());

            var data = connectionInformix.GetData("SELECT * FROM user_web");
            Console.WriteLine($"\nRows: {data.Rows.Count}");

            Console.ReadKey();
        }
    }
}
