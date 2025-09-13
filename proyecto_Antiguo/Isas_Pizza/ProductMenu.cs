using System.Collections.Generic;
using System.Linq;
using Isas_Pizza.Models;

namespace Isas_Pizza
{
    public static class ProductMenu
    {
        public static IEnumerable<Producto> AvailableProducts
        (
            IEnumerable<Producto> productos,
            IEnumerable<IngredienteEnStock> inventario
        )
        {
            Dictionary<string, IngredienteEnStock> invArr
                = inventario.ToDictionary(ies => ies.ingrediente.nombre);
            return productos.ToList().FindAll(p =>
                p.ingredientesRequeridos.All(ir =>
                    invArr.ContainsKey(ir.ingrediente.nombre) &&
                    invArr.GetValueOrDefault(ir.ingrediente.nombre).cantidad > ir.cantidad)
            );
        }
    }
}