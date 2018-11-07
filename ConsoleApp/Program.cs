using ConsoleApp.Models;
using CORE.Extensions;
using CORE.Methods;
using CORE.Models;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            // add comment for test connection github
            var queryMSS = "usp_CRUDTest01 ";
            var queryMSSFindCountry = "SELECT nIdPais01 FROM [SIR].[SIR].[SIR_01_PAISES] WHERE sClavePais = '{0}'";
            var queryInformix = "SELECT no_cliente, clave, descripcion, fracc_ara, pais, fecha, catego, umtar, cd FROM partscat WHERE fecha >= '06112018'";

            //var responseMSS = Sql.ExecuteReader<Sir_99_Partes>(queryMSS, new ConnectionMicrosoftSqlServer());
            var responseInformix = Informix.ExecuteReader<PartsCat>(queryInformix, new ConnectionInformix());

            if (responseInformix.Status.Equals(Status.Success))
            {
                var parts = new List<Sir_99_Partes>();
                (responseInformix.Data as List<PartsCat>).ForEach(part =>
                {
                    parts.Add(new Sir_99_Partes
                    {
                        NIdCliente = part.No_Cliente, // Este no es el bueno... es el de una segunda tabla XD
                        SParte = part.Clave,
                        SDescripcionFactura = part.Descripcion,
                        SDescripcionAA = part.Descripcion,
                        SFraccion = part.Fracc_Ara,
                        NIdPaisOriDest01 = 0, // Sql.ExecuteReader<Sir_99_Partes>(string.Format(queryMSSFindCountry, part.Pais), new ConnectionMicrosoftSqlServer())
                        DFechaAlta = part.Fecha,
                        NIdCategoria = part.Catego,
                        NIdUMT10 = part.UmTar
                        // = part.Cd
                    });
                });

                var index = 0;
                parts.ForEach(part =>
                {
                    var response = Sql.ExecuteNonQuery("INSERT INTO ", new ConnectionMicrosoftSqlServer());

                    index++;
                    Console.WriteLine($"{index}.- Número de Parte: {part.SParte}");
                });
            }
            else
            {
                Console.WriteLine(responseInformix.Error);
            }

            Console.ReadKey();
        }
    }
}
