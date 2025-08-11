using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

namespace Isas_Pizza
{
    public enum EstadoOrden
    {
        ORDENADA,
        COCINANDO,
        LISTA,
        ENTREGADA
    }

    public static class EstadoOrdenExtensions
    {
        public static string GetString(this EstadoOrden estado)
            => estado.ToString("G").ToLower();
    }

    /// <summary>
    /// Orden que un consumidor puede Solicitar.
    /// </summary>
    public record class Orden
    {
        [Range(0, int.MaxValue)]
        public int numeroOrden {get; init;}

        public EstadoOrden estado { get; init; }
        /// <summary>
        /// Lista de productos y cantidades por producto que el cliente ordenó.
        /// </summary>
        [ProductosCantidadPositiva(ErrorMessage = "{0} debe tener cantidades positivas")]
        public ICollection<(Producto producto, int cantidad)> productosOrdenados {get; init;}
        /// <summary>
        /// Fecha/Hora en la cual se realizó la orden.
        /// </summary>
        public DateTime ordenadaEn { get; init; } = DateTime.Now;
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
