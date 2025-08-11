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

            //colección de IngredienteCantidad
            foreach (var producto in elements)
            {
                var ingredientes = producto.ingredientesRequeridos
                    .Select(i => $"{i.cantidad} {i.ingrediente.unidad} de {i.ingrediente.nombre}");

                Console.WriteLine(
                    $"\n{producto.nombre.ToUpper()}\n" +
                    $"Ingredientes: {string.Join(", ", ingredientes)}"
                );
            }

            Console.WriteLine(new string('=', 25));
        }

        
    }
}
