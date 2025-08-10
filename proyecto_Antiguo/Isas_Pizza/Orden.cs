using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Isas_Pizza
{
    /// <summary>
    /// Orden que un consumidor puede Solicitar.
    /// </summary>
    public record class Orden
    {
        /// <summary>
        /// Lista de productos y cantidades por producto que el cliente ordenó.
        /// </summary>
        [ProductosCantidadPositiva(ErrorMessage = "{0} debe tener cantidades positivas")]
        public ICollection<(Producto producto, int cantidad)> productosOrdenados {get; init;}
        /// <summary>
        /// Fecha/Hora en la cual se realizó la orden.
        /// </summary>
        public DateTime ordenadaEn {get;} = DateTime.Now;
    }

    
    /// <summary>
    /// Validador de si todas las cantidades de los productos de las
    /// órdenes son positivas
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ProductosCantidadPositivaAttribute : ValidationAttribute
    {
        public override bool IsValid(object productosOrdenados)
            => ((ICollection<(Producto producto, int cantidad)>) productosOrdenados)
                .All(t => t.cantidad > 0);
    }
}
