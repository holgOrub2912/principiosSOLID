using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Isas_Pizza
{
    public enum Unidad
    {
        UNIDAD,
        LIBRA,
        GRAMO,
        LITRO,
        MILILITRO,
    }

    public static class UnidadExtensions
    {
        public static string GetString(this Unidad unidad, bool singular)
            => (unidad, singular) switch
        {
            // Manejar casos de pluralidad especiales.
            (Unidad.UNIDAD, false) => "unidades",
            (_, true) => unidad.ToString("G").ToLower(),
            (_, false) => unidad.ToString("G").ToLower() + "s",
        };
    }
    
    public record Ingrediente
    {
        [Required(ErrorMessage = "{0} no debe estar vacío")]
        public string nombre {get; init;}
        public string descripcion {get; init;}
        public Unidad unidad {get; init;}
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UnidadCantidadEnteraAttribute : ValidationAttribute
    {
        public override bool IsValid(object ingCantidadObj)
        {
            IngredienteCantidad ingCant = (IngredienteCantidad) ingCantidadObj;
            return ingCant.ingrediente.unidad != Unidad.UNIDAD ||
                   ingCant.cantidad == (int) ingCant.cantidad;
        }
    }

    [UnidadCantidadEntera(ErrorMessage = "cantidad debe ser especificado en unidades enteras")]
    public record IngredienteCantidad
    {
        public Ingrediente ingrediente {get; init;}
        [Range(0.0, Double.MaxValue, ErrorMessage = "{0} no puede ser negativo")]
        public double cantidad {get; init;}

        public static ValidationResult validarCantidadUnidades(
            IngredienteCantidad ingCant
        ) =>   (ingCant.ingrediente.unidad != Unidad.UNIDAD ||
                ingCant.cantidad == (int) ingCant.cantidad)
             ? ValidationResult.Success
             : new ValidationResult("Cantidad inválida");
    }

    public record IngredienteEnStock: IngredienteCantidad
    {
        [CustomValidation(typeof(IngredienteEnStock), "ValidarFechaFutura")]
        public DateTime fechaVencimiento {get; init;}

        public static ValidationResult ValidarFechaFutura(DateTime fecha)
            => fecha > DateTime.Today
             ? ValidationResult.Success
             : new ValidationResult($"{nameof(fecha)} debe de estar en el futuro");
    }
}
