using System.Collections.Generic;
using System.Linq;
using Isas_Pizza.Models;

namespace Isas_Pizza {
    public class InventarioChecker
    (
        Dictionary<string, double> inventario
    ) : ICounterVisitor<bool, Orden>,
        ICounterVisitor<bool, Producto>,
        ICounterVisitor<bool, Ingrediente>
    {
        public bool Visit(Orden orden, double cantidad)
            => orden.productosOrdenados.Aggregate(
                true,
                (acc, it) => acc && this.Visit(it.producto, it.cantidad * cantidad)
            );


        public bool Visit(Producto producto, double cantidad)
            => producto.ingredientesRequeridos.Aggregate(
                true,
                (acc, it) => acc && this.Visit(it.ingrediente, it.cantidad * cantidad)
            );
        
        public bool Visit(Ingrediente ingrediente, double cantidad)
            => (inventario[ingrediente.nombre] =
                (inventario.GetValueOrDefault(ingrediente.nombre, 0)
                 - cantidad)) >= 0;
    }
}