//#define Developing
//#define Replicate
#define LoadFileOperations
using ConsoleApp.Extensions;
using ConsoleApp.Models;
using CORE.Methods;
using CORE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ConsoleApp
{
    public class Program
    {
        private static readonly string directory = @"C:\ReplicatorParts";
        private static readonly string file = "Operations.txt";
        private static string path = $@"{directory}\{file}";
        private static int countPartsInsertadas = 0;
        private static int countCustomerPartInsertados = 0;

        private static void Main(string[] args)
        {
#if Developing
            WriteLineFile("hola");
#endif

#if Replicate
            var queryParts = "SELECT Pa.clave AS NumeroParte, Pa.descripcion, Pa.fracc_ara AS FraccionArancelaria, Pa.pais, Pa.fecha, Pa.catego AS Categoria, Pa.cd AS Ciudad, " +
                                    "Cl.no_cliente AS NumeroCliente, Cl.nombre_cliente AS NombreCliente, Cl.rfc_cliente AS RfcCliente, Cl.domicilio_cliente AS DomicilioCliente, Cl.colonia AS ColoniaCliente, Cl.no_interior AS NumeroInteriorCliente, Cl.no_exterior AS NumeroExteriorCliente, Cl.codigo AS CodigoPostalCliente, Cl.ciudad_cliente AS CiudadCliente, Cl.ent_federativa AS EntidadFederativaCliente, Cl.pais AS PaisCliente, " +
                                    "Pa.no_provee AS NumeroProveedor, " +
                                    "Un.unidad AS ClaveUMT, Un.descripcion_u AS DescripcionUMT " +
                             "FROM partscat Pa " +
                                  "INNER JOIN cusmasm3 Cl ON (Cl.no_cliente = Pa.no_cliente) " +
                                  "INNER JOIN unidad Un ON (Un.unidad = Pa.umtar) " +
                             "WHERE Pa.fecha >= '01102018' AND Pa.fecha <= '31102018'";
            var querySupplier = "SELECT no_provee AS Numero, nombre_provee AS Nombre, rfc_provee AS Rfc, domicilio_provee AS Domicilio, num_interior AS NumeroInterior, num_exterior AS NumeroExterior, codigo_postal AS CodigoPostal, ciudad_provee AS Ciudad, entidad AS EntidadFederativa, pais AS Pais " +
                                "FROM {0} " +
                                "WHERE no_provee = '{1}'";

            var responseInformix = Informix.ExecuteReader<PartsCat>(queryParts, new ConnectionInformix());
            switch (responseInformix.Status)
            {
                case Status.Exception:
                    WriteLineFile($"Information: Desconocido | Description: {responseInformix.Description}", 1);
                    break;
                case Status.Success:
                    var parts = responseInformix.Data as List<PartsCat>;
                    parts.ForEach(part => // Loop para llenar el proveedor en cada parte.
                    {
                        part.Trim();

                        var responseFindCustomer = part.FindCustomer();
                        switch (responseFindCustomer.Status)
                        {
                            case Status.Exception:
                                WriteLineFile($"Information: {part.PartToString()} | Description: {responseFindCustomer.Description}", 1);
                                break;
                            case Status.Success:
                                bool flagFindSupplier = (part.NumeroProveedor ?? "").Equals("9999");
                                if (!flagFindSupplier)
                                {
                                    var tableSupplier = "";
                                    switch (part.Ciudad)
                                    {
                                        case 1: // Nuevo Laredo
                                        case 2: // Laredo Tx
                                        case 6: // Altamira
                                            tableSupplier = "provm3";
                                            break;
                                        case 3: // San Luis Potosí
                                            tableSupplier = "provslpm3";
                                            break;
                                        case 4: // Veracruz
                                        case 12: // Veracruz, Ver. Patente 3850
                                            tableSupplier = "provrrm3";
                                            break;
                                        case 5: // Matamoros
                                            break;
                                        case 7: // México
                                            tableSupplier = "provmxm3";
                                            break;
                                        case 8: // Hidalgo
                                            tableSupplier = "provchm3";
                                            break;
                                        case 9: // Manzanillo
                                            tableSupplier = "provmanm3";
                                            break;
                                        case 10: // Lázaro Cárdenas
                                            tableSupplier = "provlcm3";
                                            break;
                                        case 11: // Quéretaro
                                            break;
                                        case 27: // LOGINSPEC LAREDO              
                                            break;
                                        case 28: // CESAR RAMOS  LAREDO           
                                            break;
                                        case 29: // CESAR RAMOS  SLP              
                                            break;
                                        case 31: // Silao, Guanajuato
                                            break;
                                            //0, 81, 82, 101, 3850, <null>
                                    }

                                    if (!string.IsNullOrEmpty(tableSupplier))
                                    {
                                        var responseSupplier = Informix.ExecuteReader<Supplier>(string.Format(querySupplier, tableSupplier, part.NumeroProveedor), new ConnectionInformix());
                                        switch (responseSupplier.Status)
                                        {
                                            case Status.Exception:
                                                WriteLineFile($"Information: {part.PartToString()} | Description: {responseSupplier.Description}", 1);
                                                break;
                                            case Status.Success:
                                                part.Supplier = ((List<Supplier>)responseSupplier.Data).FirstOrDefault();
                                                if (part.Supplier != null)
                                                {
                                                    part.Supplier.Trim();

                                                    var responseFindSupplier = part.FindSupplier();
                                                    switch (responseFindSupplier.Status)
                                                    {
                                                        case Status.Exception:
                                                            WriteLineFile($"Information: {part.PartToString()} | Description: {responseFindSupplier.Description}", 1);
                                                            break;
                                                        case Status.Success:
                                                            ContinueOp(part);
                                                            break;
                                                        case Status.Warning:
                                                            WriteLineFile($"Information: {part.PartToString()} | Description: {responseFindSupplier.Description}", 3);
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    WriteLineFile($"Information: {part.PartToString()} | Description: No se encontró el proveedor en ERYCIA1.", 3);
                                                }
                                                break;
                                            case Status.Warning:
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {                                        
                                        WriteLineFile($"Information: {part.PartToString()} | Description: No hay una tabla definida para buscar la ciudad.", 3);
                                    }
                                }
                                else
                                {
                                    ContinueOp(part);
                                }
                                break;
                            case Status.Warning:
                                WriteLineFile($"Information: {part.PartToString()} | Description: {responseFindCustomer.Description}", 3);
                                break;
                            default:
                                break;
                        }
                    });

                    WriteLineFile($"Partes encontradas: {parts.Count} - Partes insertadas: {countPartsInsertadas} - Cliente-Parte insertados: {countCustomerPartInsertados}", 0);
                    break;
                case Status.Warning:
                    break;
                default:
                    break;
            }
#endif

#if LoadFileOperations
            var october = "01-10-2018 al 31-10-2018";
            var november = "01-11-2018 al 07-11-2018";
            path = $@"{directory}\{november}\{file}";

            if (File.Exists(path))
            {                
                var text=File.ReadAllText(path);
                var lines = text.Split('¶');
                //var exceptions = lines.ToList().FindAll(s => s.Contains("The statement has been terminated. ")); // usar este porque no se agrego EXCEPTION
                var exceptionsArray = lines.ToList().FindAll(s => s.Contains("Exception")); // cambiar a EXCEPTION
                //var warnings = lines.ToList().FindAll(s => s.Contains("Description: No se") || s.Contains("Description: No hay una") || s.Contains("Description: La parte") || s.Contains("Description: No devolvió")); // usar este porque no se agrego WARNING
                var warningsArray = lines.ToList().FindAll(s => s.Contains("Warning")); // cambiar a WARNING
                //var inserts = lines.ToList().FindAll(s => s.Contains("Description: NULL")); // usar este porque nose agrego INSERT
                var insertsArray = lines.ToList().FindAll(s => s.Contains("Insert")); // cambiar a INSERT

                foreach (var line in lines)
                {
                    var exception = exceptionsArray.FirstOrDefault(e => e.Equals(line));
                    if (exception != null)
                        continue;

                    var warning = warningsArray.FirstOrDefault(w => w.Equals(line));
                    if (warning != null)
                        continue;

                    var insert = insertsArray.FirstOrDefault(i => i.Equals(line));
                    if (insert != null)
                        continue;

                    warningsArray.Add(line);
                }

                //var exceptions = new List<RPOperations>();
                //exceptionsArray.ForEach(exception =>
                //{
                //    //var dateTimeExecution = exception
                //    exceptions.Add(new RPOperations
                //    {
                //        IdRPStatus = 1, // EXCEPTION
                //        DateTimeRegistration = DateTime.Now,
                //        DateTimeExecution = DateTime.Now,
                //        PartInformation = "",
                //        Description = ""
                //    });
                //});

                var stop = 0;
            }
#endif
            //NIdPaisOriDest01 = 0, // Sql.ExecuteReader<Sir_99_Partes>(string.Format(queryMSSFindCountry, part.Pais), new ConnectionMicrosoftSqlServer())

            Console.ReadKey();
        }

        private static void WriteLineFile(string text, int option)
        {
            Console.ForegroundColor = ConsoleColor.White;

            var initialText = "";
            if (!Directory.Exists(directory))            
                Directory.CreateDirectory(directory);            

            switch (option)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("EXCEPTION:");
                    initialText = "EXCEPTION: {0} {1}";
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("INSERT:");
                    initialText = "INSERT: {0} {1}";
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("WARNING:");
                    initialText = "WARNING: {0} {1}";
                    break;
                default:
                    initialText = "FIN: {0} {1}";
                    break;
            }            

            Console.WriteLine($"{string.Format(initialText, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text)} ¶");
            File.AppendAllText(path, $"{string.Format(initialText, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text)} ¶");
        }

        private static void ContinueOp(PartsCat part)
        {
            var responseFindUMT = part.FindUMT();
            switch (responseFindUMT.Status)
            {
                case Status.Exception:
                    WriteLineFile($"Information: {part.PartToString()} | Description: {responseFindUMT.Description}", 1);
                    break;
                case Status.Success:
                    part.TipoOperacion = part.NumeroProveedor.Equals("9999") ? 2 : 1;
                    var queryFindPart = "SELECT Pa.nIdParte99 " +
                                        "FROM [SIR].[SIR].[SIR_99_PARTES] Pa " +
                                            "INNER JOIN [SIR].[SIR].[SIR_383_RELAC_CLIENTE_PARTE] ReClPa ON ReClPa.nIdParte99 = Pa.nIdParte99 " +
                                       $"WHERE ReClPa.nIdCliente = {part.NumeroCliente} " +
                                           $"AND Pa.sParte = '{part.NumeroParte}' " +
                                           $"AND Pa.sFraccion = '{part.FraccionArancelaria}' " +
                                           $"AND Pa.nIdProveedor42 {(part.NumeroProveedor.Equals("9999") ? "IS NULL" : $"= {part.Supplier?.Numero ?? ""}")} " + //= {part.NumeroProveedor}
                                           $"AND nTipoOperacion = {part.TipoOperacion} " +
                                           $"AND Pa.bActiva = 1";
                    var responseFindPart = Sql.ExecuteReader(queryFindPart, new ConnectionMicrosoftSqlServer());
                    switch (responseFindPart.Status)
                    {
                        case Status.Exception:
                            WriteLineFile($"Information: {part.PartToString()} | Description: {responseFindPart.Description}", 1);
                            break;
                        case Status.Success:
                            var rowsFindPart = (responseFindPart.Data as DataTable).Rows;
                            if (rowsFindPart.Count > 0)
                            {
                                WriteLineFile($"Information: {part.PartToString()} | Description: La parte ya se encuentra en la base de datos.", 3);
                            }
                            else
                            {
                                var queryInsertPart = $"EXECUTE [uspInsertPartFromErycia] '{part.NumeroParte}', " +
                                                                                       $"'{part.Descripcion}', " +
                                                                                       $"'{part.Descripcion}', " +
                                                                                       $"'{part.FraccionArancelaria}', " +
                                                                                       $"'{part.Fecha.ToString("yyyy-MM-dd")}', " +
                                                                                        $"{part.Categoria}, " +
                                                                                        $"{part.ClaveUMT}, " +
                                                                                        $"{(part.NumeroProveedor.Equals("9999") ? "NULL" : $"{part.Supplier?.Numero ?? "NULL"}")}, " +
                                                                                        $"{part.TipoOperacion}";
                                var responseInsertPart = Sql.ExecuteNonStoredProcedure(queryInsertPart, new ConnectionMicrosoftSqlServer());
                                switch (responseInsertPart.Status)
                                {
                                    case Status.Exception:
                                        WriteLineFile($"Information: {part.PartToString()} | Description: {responseInsertPart.Description}", 1);
                                        break;
                                    case Status.Success:
                                        countPartsInsertadas++;
                                        WriteLineFile($"Information: {part.PartToString()} | Description: NULL ", 2);

                                        var rows = (responseInsertPart.Data as DataTable).Rows;
                                        var idParte = rows.Count > 0 ? rows[0]?.ItemArray[0]?.ToString() ?? "" : "";
                                        if (!string.IsNullOrEmpty(idParte))
                                        {
                                            var queryInsertCustomerPart = "INSERT INTO [SIR].[SIR].[SIR_383_RELAC_CLIENTE_PARTE] (nIdParte99, nIdCliente) " +
                                                                                                                   $"VALUES ({idParte}, {part.NumeroCliente})";
                                            var responseInsertCustomerPart = Sql.ExecuteNonQuery(queryInsertCustomerPart, new ConnectionMicrosoftSqlServer());
                                            switch (responseInsertCustomerPart.Status)
                                            {
                                                case Status.Exception:
                                                    WriteLineFile($"Information: {part.PartToString()} | Description: {responseInsertCustomerPart.Description}", 1);
                                                    break;
                                                case Status.Success:
                                                    countCustomerPartInsertados++;
                                                    WriteLineFile($"Information: nIdParte99: {idParte} nIdCliente: {part.NumeroCliente} | Description: NULL ", 2);
                                                    break;
                                                case Status.Warning:
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            WriteLineFile($"Information: {part.PartToString()} | Description: No devolvió el indice de la parte insertada.", 3);
                                        }
                                        break;
                                    case Status.Warning:
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Status.Warning:
                            break;
                        default:
                            break;
                    }
                    break;
                case Status.Warning:
                    WriteLineFile($"Information: {part.PartToString()} | Description: {responseFindUMT.Description}", 3);
                    break;
                default:
                    break;
            }
        }
    }
}
