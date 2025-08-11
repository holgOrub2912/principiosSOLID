using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class IngredienteIO : IBlockingDisplayer<IngredienteEnStock>, IBlockingPrompter<IngredienteEnStock>
    {

        private readonly ICollection<IngredienteEnStock> _ingredientes;

        // Constructor específico para ingredientes
        public IngredienteIO(ICollection<IngredienteEnStock> ingredientes)
        {
            _ingredientes = ingredientes ?? throw new ArgumentNullException(nameof(ingredientes));
        }

        public IngredienteEnStock Ask(IngredienteEnStock? _)
        {
            if (_ingredientes.Count == 0)
            {
                throw new InvalidOperationException("No hay ingredientes registrados");
            }

            Console.WriteLine("\n=== Seleccione un Ingrediente ===");

            int index = 1;
            foreach (var ingrediente in _ingredientes)
            {
                Console.WriteLine($"{index}. {ingrediente.ingrediente.nombre} - Vence: {ingrediente.fechaVencimiento:d}");
                index++;
            }

            IngredienteEnStock ingredienteSeleccionado;
            while (true)
            {
                Console.Write("\nIngrese el nombre del ingrediente deseado: ");
                string nombreIngrediente = Console.ReadLine()?.Trim() ?? string.Empty;

                // Validación del nombre
                var validationResults = new List<ValidationResult>();
                var ingredienteTest = new Ingrediente { nombre = nombreIngrediente };
                bool isValid = Validator.TryValidateObject(
                    ingredienteTest,
                    new ValidationContext(ingredienteTest),
                    validationResults,
                    true
                );

                if (!isValid)
                {
                    Console.WriteLine(
                        "Se presentaron los siguientes errores:\n" +
                        string.Join("\n", validationResults.Select(vr => vr.ErrorMessage))
                    );
                    continue;
                }

                // Buscar ingrediente (insensible a mayúsculas y con coincidencia parcial)
                ingredienteSeleccionado = _ingredientes.FirstOrDefault(i =>
                    i.ingrediente.nombre.Contains(nombreIngrediente, StringComparison.OrdinalIgnoreCase));

                if (ingredienteSeleccionado != null)
                {
                    break;
                }

                Console.WriteLine("Ingrediente no encontrado. Intente nuevamente.");
                Console.WriteLine("Sugerencias:");
                var sugerencias = _ingredientes
                    .Where(i => i.ingrediente.nombre.Contains(nombreIngrediente, StringComparison.OrdinalIgnoreCase))
                    .Take(3);

                if (sugerencias.Any())
                {
                    foreach (var sug in sugerencias)
                    {
                        Console.WriteLine($"- {sug.ingrediente.nombre} (Vence: {sug.fechaVencimiento:d})");
                    }
                }
                else
                {
                    Console.WriteLine("No se encontraron sugerencias similares");
                }
            }

            return ingredienteSeleccionado;
        }

        public void Display(ICollection<IngredienteEnStock> elements)
        {
            Console.WriteLine("=== Ingredientes ===");
            foreach (var ingrediente in elements)
            {
                Console.WriteLine($"- {ingrediente.ingrediente.nombre}, Vence: {ingrediente.fechaVencimiento:d}");
            }
        }
        

        
    }
}
