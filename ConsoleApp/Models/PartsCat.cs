using System;

namespace ConsoleApp.Models
{
    public class PartsCat
    {
        public string NumeroParte { get; set; }
        public string Descripcion { get; set; }              
        public string FraccionArancelaria { get; set; }
        public string Pais { get; set; }
        public DateTime Fecha { get; set; }
        public int Categoria { get; set; }
        public int ClaveUMT { get; set; }
        public string DescripcionUMT { get; set; }
        public int Ciudad { get; set; }
        public int NumeroCliente { get; set; }
        public string NombreCliente { get; set; }
        public string RfcCliente { get; set; }
        public string DomicilioCliente { get; set; }
        public string ColoniaCliente { get; set; }
        public string NumeroInteriorCliente { get; set; }
        public string NumeroExteriorCliente { get; set; }
        public string CodigoPostalCliente { get; set; }
        public string CiudadCliente { get; set; }
        public string EntidadFederativaCliente { get; set; }
        public string PaisCliente { get; set; }
        public string NumeroProveedor { get; set; }
        public int TipoOperacion { get; set; }

        public Supplier Supplier { get; set; }
    }
}
