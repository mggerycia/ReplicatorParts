using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp.Extensions
{
    public static class SupplierExtensions
    {
        public static void Trim(this Supplier supplier)
        {
            supplier.Ciudad?.Trim();
            supplier.CodigoPostal?.Trim();
            supplier.Domicilio?.Trim();
            supplier.EntidadFederativa?.Trim();
            supplier.Nombre?.Trim();
            supplier.Numero?.Trim();
            supplier.NumeroExterior?.Trim();
            supplier.NumeroInterior?.Trim();
            supplier.Pais?.Trim();
            supplier.Rfc?.Trim();            
        }        
    }
}
