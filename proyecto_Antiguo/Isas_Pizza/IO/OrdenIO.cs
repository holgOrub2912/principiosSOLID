using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class OrdenIO : IBlockingDisplayer<Orden>, IBlockingPrompter<Orden>
    {

        private readonly ICollection<Orden> _ordenes;

        public OrdenIO(ICollection<Orden> ordenes)
        {
            _ordenes = ordenes ?? throw new ArgumentNullException(nameof(ordenes));
        }

        public Orden Ask(Orden? _)
        {
            if (_ordenes.Count == 0)
            {
                throw new InvalidOperationException("No hay órdenes registradas");
            }

            Console.WriteLine("\n=== Búsqueda de Orden ===");

            //Mostrar ordenes
           
            foreach (var orden in _ordenes.OrderByDescending(o => o.ordenadaEn).Take(5))
            {
                Console.WriteLine($"#{orden.numeroOrden} - {orden.estado.GetString()} - {orden.ordenadaEn:g}");
            }

            Orden ordenSeleccionada;
            while (true)
            {
                Console.Write("\nIngrese el número de orden: ");
                string input = Console.ReadLine()?.Trim() ?? string.Empty;

                // Validación del número de orden
                var validationResults = new List<ValidationResult>();
                var ordenTest = new Orden { numeroOrden = int.TryParse(input, out int num) ? num : -1 };
                bool isValid = Validator.TryValidateProperty(
                    ordenTest.numeroOrden,
                    new ValidationContext(ordenTest) { MemberName = nameof(Orden.numeroOrden) },
                    validationResults
                );

                if (!isValid)
                {
                    Console.WriteLine(
                        "Se presentaron los siguientes errores:\n" +
                        string.Join("\n", validationResults.Select(vr => vr.ErrorMessage))
                    );
                    continue;
                }

                // Buscar orden por número
                ordenSeleccionada = _ordenes.FirstOrDefault(o =>
                    o.numeroOrden == ordenTest.numeroOrden);

                if (ordenSeleccionada != null)
                {
                    break;
                }

                Console.WriteLine("Orden no encontrada. Intente nuevamente.");
                Console.WriteLine("Sugerencias (últimas 3 órdenes):");
                var sugerencias = _ordenes
                    .OrderByDescending(o => o.ordenadaEn)
                    .Take(3);

                foreach (var sug in sugerencias)
                {
                    Console.WriteLine($"- Orden #{sug.numeroOrden} ({sug.ordenadaEn:g})");
                }
            }

            return ordenSeleccionada;
        }

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

       
    }
}
