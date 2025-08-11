using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class OrdenIO : IBlockingSelector, IBlockingDisplayer<Orden>
    {
        public void Display(ICollection<Orden> elements)
        {
            Console.WriteLine("\n=== HISTORIAL DE ÓRDENES ===");

            foreach (var orden in elements.OrderBy(o => o.ordenadaEn))
            {
                Console.WriteLine($"\nOrden realizada: {orden.ordenadaEn:g}");
                Console.WriteLine("Productos:");

                foreach (var (producto, cantidad) in orden.productosOrdenados)
                {
                    Console.WriteLine($"- {cantidad}x {producto.nombre}");
                }
            }

            Console.WriteLine(new string('=', 30));
        }
        public T SelectOne<T>(ICollection<(string label, T option)> options)
        {
            if (options == null || options.Count == 0)
                throw new ArgumentException("No hay opciones disponibles para seleccionar");

            Console.WriteLine("\n=== SELECCIÓN DE ORDEN ===");

            var optionsList = options
                .Select((x, i) => $"{i + 1}. {x.label}")
                .ToList();

            Console.WriteLine(string.Join("\n", optionsList));

            int selectedIndex;
            while (true)
            {
                Console.Write($"Seleccione una orden (1-{options.Count}): ");
                if (int.TryParse(Console.ReadLine(), out selectedIndex) &&
                    selectedIndex >= 1 &&
                    selectedIndex <= options.Count)
                {
                    break;
                }
                Console.WriteLine("¡Opción inválida! Intente nuevamente.");
            }

            return options.ElementAt(selectedIndex - 1).option;
        }
    }
}
