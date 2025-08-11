using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class IngredienteIO : IBlockingDisplayer<IngredienteEnStock>, IBlockingPrompter<IngredienteEnStock>, IBlockingSelector
    {
        public void Display(ICollection<IngredienteEnStock> elements)
        {
            Console.WriteLine("=== Ingredientes ===");
            foreach (var ingrediente in elements)
            {
                Console.WriteLine($"- {ingrediente.ingrediente.nombre}, Vence: {ingrediente.fechaVencimiento:d}");
            }
        }
        public IngredienteEnStock Ask() {

            Console.WriteLine("Nuevo stock");
            // Solicitar información del ingrediente base
            Console.Write("Nombre del ingrediente: ");
            string nombre = Console.ReadLine() ?? string.Empty;

            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine() ?? string.Empty;

            Console.Write("Unidad (UNIDAD/LIBRA/GRAMO/LITRO): ");
            Unidad unidad = Enum.Parse<Unidad>(Console.ReadLine() ?? "UNIDAD");

            // Solicitar información específica de IngredienteEnStock
            Console.Write("Cantidad: ");
            double cantidad = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("Fecha de vencimiento (yyyy-MM-dd): ");
            DateTime fechaVencimiento = DateTime.Parse(Console.ReadLine() ?? DateTime.Today.AddDays(1).ToString());

            // Crear y validar el objeto
            var ingrediente = new IngredienteEnStock
            {
                ingrediente = new Ingrediente
                {
                    nombre = nombre,
                    descripcion = descripcion,
                    unidad = unidad
                },
                cantidad = cantidad,
                fechaVencimiento = fechaVencimiento
            };

            // Validar el objeto
            var resultados = new List<ValidationResult>();
            if (!Validator.TryValidateObject(ingrediente, new ValidationContext(ingrediente), resultados, true))
            {
                Console.WriteLine("Errores de validación:");
                foreach (var error in resultados)
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
                }
                throw new ValidationException("El ingrediente no cumple con las validaciones requeridas");
            }

            return ingrediente;
        }

        public T SelectOne<T>(ICollection<(string label, T option)> options) {
            if (options == null || options.Count == 0)
                throw new ArgumentException("No hay opciones para seleccionar");
            Console.WriteLine("\n=== Seleccione una opción ===");

            // Mostrar opciones
            int index = 1;
            foreach (var (label, _) in options)
            {
                Console.WriteLine($"{index}. {label}");
                index++;
            }

            // Validar entrada
            int selectedIndex;
            while (true)
            {
                Console.Write("Ingrese el número de opción: ");
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
