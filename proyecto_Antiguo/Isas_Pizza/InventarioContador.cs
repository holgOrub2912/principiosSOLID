using System.Collections.Generic;
using System.Linq;

namespace Isas_Pizza {
    public class InventarioChecker
    (
        Dictionary<string, double> inventario
    ) : ICounterVisitor<bool>
    {
        public bool Visit(Orden orden)
            => this.Visit(orden, 1);
        public bool Visit(Orden orden, int cantidad)
            => orden.productosOrdenados.Aggregate(
                true,
                (acc, it) => acc && this.Visit(it.producto, it.cantidad * cantidad)
            );


        public bool Visit(Producto producto, int cantidad)
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