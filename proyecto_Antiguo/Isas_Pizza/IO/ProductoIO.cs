using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class ProductoIO: IBlockingDisplayer<Producto>
    {
        

        public void Display(ICollection<Producto> elements)
        {
            Console.WriteLine("\n=== MENÚ DE PRODUCTOS ===");

            int maxNameLength = elements.Max(p => p.nombre.Length);
            // colección de IngredienteCantidad
            foreach (var producto in elements)
            {
                var ingredientes = producto.ingredientesRequeridos
                    .Select(i => $"- {i.ingrediente.nombre}");

                Console.WriteLine(
                    String.Format(
                        "{0,-" + maxNameLength.ToString() +"} {1,10}",
                        producto.nombre, producto.precio
                    ) +
                    $"\nIngredientes: {string.Join("\n", ingredientes)}"
                );
            }

            Console.WriteLine(new string('=', 25));
        }

        
    }
}
