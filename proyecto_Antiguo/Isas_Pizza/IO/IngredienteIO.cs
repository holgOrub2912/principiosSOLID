using Isas_Pizza.Persistence.EFModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Isas_Pizza.Persistence;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Isas_Pizza.IO
{
    public class IngredienteIO : IBlockingDisplayer<IngredienteEnStock>, IIngEnStockPrompter
    {

        private readonly IEnumerable<Ingrediente> _ingredientes;

        public IngredienteIO
        (
            IEnumerable<Ingrediente> ingredientes
        )
        {
            this._ingredientes = ingredientes;
        }
        

        public IngredienteEnStock Ask(Ingrediente ingredienteSeleccionado)
        {

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
                Console.WriteLine("Fecha futura (mm/dd/aaaa): ");

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

        public IngredienteEnStock Ask(IngredienteEnStock ies)
        {
            Ingrediente ingredienteSeleccionado = ies.ingrediente;
            double cantidad;
            while (true)
            {
                Console.Write("Cantidad a adicionar "
                    + $"(Ya hay {ies.cantidad} "
                    + $"{ingredienteSeleccionado.unidad.GetString(ies.cantidad == 1)} en stock): ");
                if (double.TryParse(Console.ReadLine(), out cantidad) && cantidad > 0 &&
                    (ingredienteSeleccionado.unidad != Unidad.UNIDAD || cantidad == (int)cantidad)) break;
                Console.WriteLine(ingredienteSeleccionado.unidad == Unidad.UNIDAD ?
                    "Debe ser entero positivo" : "Debe ser número positivo");
            }

            DateTime fecha;
            string input;
            do {
                fecha = ies.fechaVencimiento;
                Console.WriteLine($"Fecha futura (mm/dd/aaaa) [vacio para ({fecha.ToShortDateString()})]: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    break;
            } while (!DateTime.TryParse(input, out fecha) || fecha <= DateTime.Today);

            var nuevoIngredienteStock = new IngredienteEnStock
            {
                ingrediente = ingredienteSeleccionado,
                cantidad = ies.cantidad + cantidad,
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
                Console.WriteLine(String.Format("- {0,20} (x{1,6} {2,10}) Vence: {3}",
                    ingrediente.ingrediente.nombre,
                    ingrediente.cantidad,
                    ingrediente.ingrediente.unidad.GetString(ingrediente.cantidad == 1),
                    ingrediente.fechaVencimiento.ToShortDateString()
                ));
            }
        }
        

        
    }
}
