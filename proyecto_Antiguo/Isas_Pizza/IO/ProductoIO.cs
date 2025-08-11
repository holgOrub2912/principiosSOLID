using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class ProductoIO: IBlockingSelector, IBlockingDisplayer<Producto>
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

        public T SelectOne<T>(ICollection<(string label, T option)> options)
        {
            if (options == null || options.Count == 0)
                throw new ArgumentException("No hay opciones disponibles para seleccionar");

            Console.WriteLine("\n=== SELECCIÓN ===");

            
            var optionsList = options.Select((x, i) => $"{i + 1}. {x.label}").ToList();
            Console.WriteLine(string.Join("\n", optionsList));

            int selectedIndex;
            while (true)
            {
                Console.Write($"Seleccione (1-{options.Count}): ");
                if (int.TryParse(Console.ReadLine(), out selectedIndex) &&
                    selectedIndex >= 1 &&
                    selectedIndex <= options.Count)
                {
                    break;
                }
                Console.WriteLine("¡Opción inválida!");
            }

            return options.ElementAt(selectedIndex - 1).option;
        }
    }
}
