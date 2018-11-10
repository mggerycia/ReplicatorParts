using ConsoleApp.Models;
using CORE.Methods;
using CORE.Models;
using System;
using System.Data;

namespace ConsoleApp.Extensions
{
    public static class PartsCatExtensions
    {
        public static void Trim(this PartsCat part)
        {
            part.CiudadCliente = part.CiudadCliente?.Trim();
            part.CodigoPostalCliente = part.CodigoPostalCliente?.Trim();
            part.ColoniaCliente = part.ColoniaCliente?.Trim();
            part.Descripcion = part.Descripcion?.Trim();
            part.DomicilioCliente = part.DomicilioCliente?.Trim();
            part.EntidadFederativaCliente = part.EntidadFederativaCliente?.Trim();
            part.FraccionArancelaria = part.FraccionArancelaria?.Trim();
            part.NombreCliente = part.NombreCliente?.Trim();
            part.NumeroExteriorCliente = part.NumeroExteriorCliente?.Trim();
            part.NumeroInteriorCliente = part.NumeroInteriorCliente?.Trim();
            part.NumeroParte = part.NumeroParte?.Trim();
            part.NumeroProveedor = part.NumeroProveedor?.Trim();
            part.Pais = part.Pais?.Trim();
            part.PaisCliente = part.PaisCliente?.Trim();
            part.RfcCliente = part.RfcCliente?.Trim();
        }

        public static string BuildQueryFindCustomer(this PartsCat part, int option)
        {
            var queryFindCustomer = "";
            var entidadFederativaCustomer = "(SELECT sClave FROM [Admin].[ADMINC_02_ENTIDADES_FEDERATIVAS] WHERE nIdEntFed02 = ClDi.nIdEntFed02)";
            var paisCustomer = "(SELECT sClavePais FROM [SIR].[SIR_01_PAISES] WHERE nIdPais01 = ClDi.nIdPais01)";

            switch (option)
            {
                case 1:
                    queryFindCustomer = "SELECT Cl.nIdClie07 " +
                                        "FROM [Admin].[ADMINC_07_CLIENTES] Cl " +
                                            "INNER JOIN [Admin].[ADMINC_47_CLIENTES_DIR] ClDi ON ClDi.nIdClie07 = Cl.nIdClie07 " +
                                       $"WHERE Cl.sClave = '{part.NumeroCliente}' " +
                                            $"AND {(!string.IsNullOrEmpty(part.NombreCliente) ? $"Cl.sRazonSocial = '{part.NombreCliente}'" : "(Cl.sRazonSocial = '' OR Cl.sRazonSocial IS NULL)")} " +
                                            $"AND Cl.sRFC = '{part.RfcCliente}' " +
                                            $"AND ClDi.sCalle = '{part.DomicilioCliente}' " +
                                            $"AND {(!string.IsNullOrEmpty(part.ColoniaCliente) ? $"ClDi.sColonia = '{part.ColoniaCliente}'" : "(ClDi.sColonia = '' OR ClDi.sColonia IS NULL)")} " +
                                            $"AND {(!string.IsNullOrEmpty(part.NumeroInteriorCliente) ? $"ClDi.sNumInterior = '{part.NumeroInteriorCliente}'" : "(ClDi.sNumInterior = '' OR ClDi.sNumInterior IS NULL)")} " +
                                            $"AND ClDi.sNumExterior = '{part.NumeroExteriorCliente}' " +
                                            $"AND ClDi.sCP = '{part.CodigoPostalCliente}' " +
                                            $"AND ClDi.sCiudad = '{part.CiudadCliente}' " +
                                            $"AND {(!string.IsNullOrEmpty(part.EntidadFederativaCliente) ? $"{entidadFederativaCustomer} = '{part.EntidadFederativaCliente}'" : $"({entidadFederativaCustomer} = '' OR {entidadFederativaCustomer} IS NULL)")} " +
                                            $"AND {(!string.IsNullOrEmpty(part.PaisCliente) ? $"{paisCustomer} = '{part.PaisCliente}'" : $"({paisCustomer} = '' OR {paisCustomer} IS NULL)")} " +
                                            $"AND Cl.bStatus = 1";
                    return queryFindCustomer;
                case 2:
                    queryFindCustomer = "SELECT Cl.nIdClie07 " +
                                        "FROM [Admin].[ADMINC_07_CLIENTES] Cl " +
                                            "INNER JOIN [Admin].[ADMINC_47_CLIENTES_DIR] ClDi ON ClDi.nIdClie07 = Cl.nIdClie07 " +
                                       $"WHERE {(!string.IsNullOrEmpty(part.NombreCliente) ? $"Cl.sRazonSocial = '{part.NombreCliente}'" : "(Cl.sRazonSocial = '' OR Cl.sRazonSocial IS NULL)")} " +
                                            $"AND Cl.sRFC = '{part.RfcCliente}' " +
                                            $"AND ClDi.sCalle = '{part.DomicilioCliente}' " +
                                            $"AND {(!string.IsNullOrEmpty(part.ColoniaCliente) ? $"ClDi.sColonia = '{part.ColoniaCliente}'" : "(ClDi.sColonia = '' OR ClDi.sColonia IS NULL)")} " +
                                            $"AND {(!string.IsNullOrEmpty(part.NumeroInteriorCliente) ? $"ClDi.sNumInterior = '{part.NumeroInteriorCliente}'" : "(ClDi.sNumInterior = '' OR ClDi.sNumInterior IS NULL)")} " +
                                            $"AND ClDi.sNumExterior = '{part.NumeroExteriorCliente}' " +
                                            $"AND ClDi.sCP = '{part.CodigoPostalCliente}' " +
                                            $"AND ClDi.sCiudad = '{part.CiudadCliente}' " +
                                            $"AND {(!string.IsNullOrEmpty(part.EntidadFederativaCliente) ? $"{entidadFederativaCustomer} = '{part.EntidadFederativaCliente}'" : $"({entidadFederativaCustomer} = '' OR {entidadFederativaCustomer} IS NULL)")} " +
                                            $"AND {(!string.IsNullOrEmpty(part.PaisCliente) ? $"{paisCustomer} = '{part.PaisCliente}'" : $"({paisCustomer} = '' OR {paisCustomer} IS NULL)")} " +
                                            $"AND Cl.bStatus = 1";
                    return queryFindCustomer;
                case 3:
                    queryFindCustomer = "SELECT Cl.nIdClie07 " +
                                        "FROM [Admin].[ADMINC_07_CLIENTES] Cl " +
                                       $"WHERE Cl.sClave = '{part.NumeroCliente}' " +
                                            $"AND {(!string.IsNullOrEmpty(part.NombreCliente) ? $"Cl.sRazonSocial = '{part.NombreCliente}'" : "(Cl.sRazonSocial = '' OR Cl.sRazonSocial IS NULL)")} " +
                                            $"AND Cl.bStatus = 1";
                    return queryFindCustomer;
                default:
                    return queryFindCustomer;
            }
        }

        public static string BuildQueryFindSupplier(this PartsCat part, int option)
        {
            var queryFindSupplier = "";
            var entidadFederativaSupplier = "(SELECT sClave FROM [SIR].[SIR_284_ENTIDADES_FEDERATIVAS] WHERE nIdEntFed284 = PrDi.nIdEntFed284)";
            var paisSupplier = "(SELECT sClavePais FROM [SIR].[SIR_01_PAISES] WHERE nIdPais01 = PrDi.nIdPais01)";

            switch (option)
            {
                case 1:
                    queryFindSupplier = "SELECT Pr.nIdProveedor42 " +
                                    "FROM [SIR].[SIR_42_PROVEEDORES] Pr " +
                                        "INNER JOIN [SIR].[SIR_43_PROVEEDORES_DIR] PrDi ON PrDi.nIdProveedor42 = Pr.nIdProveedor42 " +
                                   $"WHERE {(!string.IsNullOrEmpty(part.Supplier.Numero) ? $"Pr.sClave = '{part.Supplier.Numero}'" : "(Pr.sClave = '' OR Pr.sClave IS NULL)")} " +
                                       $"AND Pr.sRazonSocial = '{part.Supplier.Nombre}' " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.Rfc) ? $"Pr.sIdFiscal = '{part.Supplier.Rfc}'" : "(Pr.sIdFiscal = '' OR Pr.sIdFiscal IS NULL)")} " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.Domicilio) ? $"PrDi.sCalle = '{part.Supplier.Domicilio}'" : "(PrDi.sCalle = '' OR PrDi.sCalle IS NULL)")} " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.NumeroInterior) ? $"PrDi.sNumInterior = '{part.Supplier.NumeroInterior}'" : "(PrDi.sNumInterior = '' OR PrDi.sNumInterior IS NULL)")} " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.NumeroExterior) ? $"PrDi.sNumExterior = '{part.Supplier.NumeroExterior}'" : "(PrDi.sNumeroExterior = '' OR PrDi.sNumeroExterior IS NULL)")} " +
                                       $"AND PrDi.sCP = '{part.Supplier.CodigoPostal}' " +
                                       $"AND PrDi.sCiudad = '{part.Supplier.Ciudad}' " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.EntidadFederativa) ? $"{entidadFederativaSupplier} = '{part.Supplier.EntidadFederativa}'" : $"({entidadFederativaSupplier} = '' OR {entidadFederativaSupplier} IS NULL)")} " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.Pais) ? $"{paisSupplier} = '{part.Supplier.Pais}'" : $"({paisSupplier} = '' OR {paisSupplier} IS NULL)")} " +
                                       $"AND Pr.bActivo = 1";
                    return queryFindSupplier;
                case 2:
                    queryFindSupplier = "SELECT Pr.nIdProveedor42 " +
                                    "FROM [SIR].[SIR_42_PROVEEDORES] Pr " +
                                        "INNER JOIN [SIR].[SIR_43_PROVEEDORES_DIR] PrDi ON PrDi.nIdProveedor42 = Pr.nIdProveedor42 " +
                                   $"WHERE Pr.sRazonSocial = '{part.Supplier.Nombre}' " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.Rfc) ? $"Pr.sIdFiscal = '{part.Supplier.Rfc}'" : "(Pr.sIdFiscal = '' OR Pr.sIdFiscal IS NULL)")} " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.Domicilio) ? $"PrDi.sCalle = '{part.Supplier.Domicilio}'" : "(PrDi.sCalle = '' OR PrDi.sCalle IS NULL)")} " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.NumeroInterior) ? $"PrDi.sNumInterior = '{part.Supplier.NumeroInterior}'" : "(PrDi.sNumInterior = '' OR PrDi.sNumInterior IS NULL)")} " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.NumeroExterior) ? $"PrDi.sNumExterior = '{part.Supplier.NumeroExterior}'" : "(PrDi.sNumeroExterior = '' OR PrDi.sNumeroExterior IS NULL)")} " +
                                       $"AND PrDi.sCP = '{part.Supplier.CodigoPostal}' " +
                                       $"AND PrDi.sCiudad = '{part.Supplier.Ciudad}' " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.EntidadFederativa) ? $"{entidadFederativaSupplier} = '{part.Supplier.EntidadFederativa}'" : $"({entidadFederativaSupplier} = '' OR {entidadFederativaSupplier} IS NULL)")} " +
                                       $"AND {(!string.IsNullOrEmpty(part.Supplier.Pais) ? $"{paisSupplier} = '{part.Supplier.Pais}'" : $"({paisSupplier} = '' OR {paisSupplier} IS NULL)")} " +
                                       $"AND Pr.bActivo = 1";
                    return queryFindSupplier;
                case 3:
                    queryFindSupplier = "SELECT Pr.nIdProveedor42 " +
                                            "FROM [SIR].[SIR_42_PROVEEDORES] Pr " +
                                           $"WHERE {(!string.IsNullOrEmpty(part.Supplier.Numero) ? $"Pr.sClave = '{part.Supplier.Numero}'" : "(Pr.sClave = '' OR Pr.sClave IS NULL)")} " +
                                                $"AND Pr.sRazonSocial = '{part.Supplier.Nombre}' " +
                                                $"AND Pr.bActivo = 1";
                    return queryFindSupplier;
                default:
                    return queryFindSupplier;
            }
        }

        public static string BuildQueryFindUMT(this PartsCat part, int option)
        {
            var queryFindUMT = "";

            switch (option)
            {
                case 1:
                    queryFindUMT = "SELECT nIdUMed10 " +
                                   "FROM [SIR].[SIR_10_UNIDADES_MEDIDA] " +
                                  $"WHERE nClave = {part.ClaveUMT} " +
                                        $"AND sDescripcion = '{part.DescripcionUMT}'";
                    return queryFindUMT;
                case 2:
                    queryFindUMT = "SELECT nIdUMed10 " +
                                   "FROM [SIR].[SIR_10_UNIDADES_MEDIDA] " +
                                  $"WHERE sDescripcion = '{part.DescripcionUMT}'";
                    return queryFindUMT;
                default:
                    return queryFindUMT;
            }
        }

        public static Response FindCustomer(this PartsCat part)
        {
            var response = new Response();
            var input=0;
            for (int option = 1; option <= 3; option++)
            {
                response = Sql.ExecuteReader(part.BuildQueryFindCustomer(option), new ConnectionMicrosoftSqlServer());
                if (response.Status.Equals(Status.Success))
                {
                    var rows = (response.Data as DataTable).Rows;
                    if (rows.Count > 0)
                    {
                        input = 1;
                        part.NumeroCliente = int.Parse(rows[0]?.ItemArray[0]?.ToString() ?? "0");

                        break;
                    }
                }
                else // Ocurrió un error en la consulta del cliente
                {
                    input = 2;
                    break;
                }
            }

            if (input.Equals(0))
            {
                response.Status = Status.Warning;
                response.Description = "No se encontró el cliente en ERYCIA2.";
            }

            return response;
        }

        public static Response FindSupplier(this PartsCat part)
        {
            var response = new Response();
            var input = 0;
            for (int index = 1; index <= 3; index++)
            {
                response = Sql.ExecuteReader(part.BuildQueryFindSupplier(index), new ConnectionMicrosoftSqlServer());
                if (response.Status.Equals(Status.Success))
                {
                    var rows = (response.Data as DataTable).Rows;
                    if (rows.Count > 0)
                    {
                        input = 1;
                        part.Supplier.Numero = rows[0]?.ItemArray[0]?.ToString() ?? "";

                        break;
                    }
                }
                else // Ocurrió un error en la búsqueda del proveedor
                {
                    input = 2;
                    break;
                }
            }

            if (input.Equals(0))
            {
                response.Status = Status.Warning;
                response.Description = "No se encontró el proveedor en ERYCIA2.";
            }

            return response;
        }

        public static Response FindUMT(this PartsCat part)
        {
            var response = new Response();
            var input = 0;
            for (int index = 1; index <= 2; index++)
            {
                response = Sql.ExecuteReader(part.BuildQueryFindUMT(index), new ConnectionMicrosoftSqlServer());
                if (response.Status.Equals(Status.Success))
                {
                    var rows = (response.Data as DataTable).Rows;
                    if (rows.Count>0)
                    {
                        input = 1;
                        part.ClaveUMT =int.Parse(rows[0]?.ItemArray[0]?.ToString() ?? "0");

                        break;
                    }
                }
                else // Ocurrió un error en la búsqueda de la UMT
                {
                    input = 2;
                    break;
                }
            }

            if (input.Equals(0))
            {
                response.Status = Status.Warning;
                response.Description = "No se encontró la UMT en ERYCIA2.";
            }

            return response;
        }

        public static string PartToString(this PartsCat part)
        {
            return $"{nameof(part.NumeroParte)}: {part.NumeroParte} " +
                   $"{nameof(part.FraccionArancelaria)}: {part.FraccionArancelaria} " +
                   $"{nameof(part.Descripcion)}Factura: {part.Descripcion} " +
                   $"{nameof(part.Descripcion)}AA: {part.Descripcion} " +
                   $"{nameof(part.ClaveUMT)}: {part.ClaveUMT} " +
                   $"{nameof(part.NumeroProveedor)}: {part.NumeroProveedor} " +
                   $"{nameof(part.Fecha)}: {part.Fecha} " +
                   $"FechaEdicion: {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} " +
                   $"{nameof(part.TipoOperacion)}: {part.TipoOperacion} " +
                   $"{nameof(part.Categoria)}: {part.Categoria}";
        }
    }
}
