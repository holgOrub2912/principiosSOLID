using Isas_Pizza.Persistence.EFModel;
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

        private readonly MenuGenericoIO _menuGenerico = new MenuGenericoIO();

        public IngredienteEnStock Ask(IngredienteEnStock? _)
        {

            //Asumo que `_` es la lista de ingredientes disponibles

            if (!(_ is ICollection<Ingrediente> ingredientesDisponibles))
                throw new ArgumentException("El parámetro debe ser una colección de Ingrediente");

            if (!ingredientesDisponibles.Any())
                throw new InvalidOperationException("No hay ingredientes disponibles para seleccionar");

            var opciones = ingredientesDisponibles
                .Select(ing => (
                    $"{ing.nombre} ({ing.unidad.GetString(false)})", 
                    ing
                ))
                .ToList();

            var ingredienteSeleccionado = _menuGenerico.SelectOne(opciones);

            double cantidad;
            while (true)
            {
                Console.Write($"Cantidad ({ingredienteSeleccionado.unidad.GetString(false)}): ");
                if (double.TryParse(Console.ReadLine(), out cantidad) && cantidad > 0 &&
                    (ingredienteSeleccionado.unidad != Unidad.UNIDAD || cantidad == (int)cantidad)) break;
                Console.WriteLine(ingredienteSeleccionado.unidad == Unidad.UNIDAD ?
                    "Debe ser entero positivo" : "Debe ser número positivo");
            }

            DateTime fecha;
            while (!DateTime.TryParse(Console.ReadLine(), out fecha) || fecha <= DateTime.Today)
                Console.WriteLine("Fecha futura (dd/mm/aaaa): ");

            var nuevoIngredienteStock = new IngredienteEnStock
            {
                ingrediente = ingredienteSeleccionado,
                cantidad = cantidad,
                fechaVencimiento = fecha
            };

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(nuevoIngredienteStock, new ValidationContext(nuevoIngredienteStock), validationResults, true))
            {
                throw new ValidationException(string.Join("\n", validationResults.Select(v => v.ErrorMessage)));
            }

            return nuevoIngredienteStock;
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
