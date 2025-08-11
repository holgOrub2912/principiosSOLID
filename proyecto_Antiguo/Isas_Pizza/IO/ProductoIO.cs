using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class ProductoIO: IBlockingDisplayer<Producto>, IBlockingPrompter
    {
        private readonly ICollection<Producto> _productos;

        public ProductoIO(ICollection<Producto> productos)
        {
            _productos = productos ?? throw new ArgumentNullException(nameof(productos));
        }

        public T Ask<T>()
        {
            // Validamos que solo se pueda preguntar por Producto
            if (typeof(T) != typeof(Producto))
            {
                throw new InvalidOperationException("Este componente solo funciona con Producto");
            }

            if (_productos.Count == 0)
            {
                throw new InvalidOperationException("No hay productos registrados");
            }

            Console.WriteLine("\n=== Búsqueda de Producto ===");
            Console.WriteLine("Productos disponibles:");

            // Mostrar cada producto con sus ingredientes
            int index = 1;
            foreach (var producto in _productos)
            {
                var ingredientes = producto.ingredientesRequeridos
                    .Select(i => $"{i.cantidad} {i.ingrediente.unidad} de {i.ingrediente.nombre}");

                Console.WriteLine($"{index}. {producto.nombre.ToUpper()}");
                Console.WriteLine($"   Ingredientes: {string.Join(", ", ingredientes)}\n");
                index++;
            }

            Producto productoSeleccionado;
            while (true)
            {
                Console.Write("\nIngrese el nombre del producto deseado: ");
                string nombreProducto = Console.ReadLine()?.Trim() ?? string.Empty;

                // Validación 
                var validationResults = new List<ValidationResult>();
                var productoTest = new Producto { nombre = nombreProducto };
                bool isValid = Validator.TryValidateObject(
                    productoTest,
                    new ValidationContext(productoTest),
                    validationResults,
                    true
                );

                if (!isValid)
                {
                    Console.WriteLine(
                        "Se presentaron los siguientes errores:\n"
                        + string.Join("\n", validationResults.Select(vr => vr.ErrorMessage))
                    );
                    continue;
                }

                // Buscar producto ( se supone insensible a mayúsculas y con coincidencia parcial)
                productoSeleccionado = _productos.FirstOrDefault(p =>
                    p.nombre.Contains(nombreProducto, StringComparison.OrdinalIgnoreCase));

                if (productoSeleccionado != null)
                {
                    break;
                }

                Console.WriteLine("Producto no encontrado. Intente nuevamente.");
                Console.WriteLine("Sugerencias:");
                var sugerencias = _productos.Where(p =>
                    p.nombre.Contains(nombreProducto, StringComparison.OrdinalIgnoreCase))
                    .Take(3);

                if (sugerencias.Any())
                {
                    foreach (var sug in sugerencias)
                    {
                        Console.WriteLine($"- {sug.nombre}");
                    }
                }
                else
                {
                    Console.WriteLine("No se encontraron sugerencias similares");
                }
            }

            return (T)(object)productoSeleccionado;
        }

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
