using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class IngredienteIO : IBlockingDisplayer<IngredienteEnStock>, IBlockingPrompter
    {

        private readonly ICollection<IngredienteEnStock> _ingredientes;

        // Constructor específico para ingredientes
        public IngredienteIO(ICollection<IngredienteEnStock> ingredientes)
        {
            _ingredientes = ingredientes ?? throw new ArgumentNullException(nameof(ingredientes));
        }

        public T Ask<T>()
        {
            // Validamos que solo se pueda preguntar por IngredienteEnStock
            if (typeof(T) != typeof(IngredienteEnStock))
            {
                throw new InvalidOperationException("Este componente solo funciona con IngredienteEnStock");
            }

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

            int selectedIndex;
            while (true)
            {
                Console.Write("Ingrese el número de opción: ");
                if (int.TryParse(Console.ReadLine(), out selectedIndex) &&
                    selectedIndex >= 1 &&
                    selectedIndex <= _ingredientes.Count)
                {
                    break;
                }
                Console.WriteLine("¡Opción inválida! Intente nuevamente.");
            }

            return (T)(object)_ingredientes.ElementAt(selectedIndex - 1);
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
