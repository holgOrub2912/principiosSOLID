using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Isas_Pizza
{
    /// <summary>
    /// Unidad con la que se mide un ingrediente.
    /// </summary>
    public enum Unidad
    {
        UNIDAD,
        LIBRA,
        GRAMO,
        LITRO,
        MILILITRO,
    }

    /// <summary>
    /// Clase extensión de Unidades, implementando operaciones útiles
    /// para manipularlas.
    /// </summary>
    public static class UnidadExtensions
    {
        /// <summary>
        /// Convertir unidad en string.
        /// </summary>
        /// <param name="unidad">Unidad a convertir.</param>
        /// <param name="singular">
        ///     Si la unidad debe estar en singular (true) o plural (false).
        /// </param>
        /// <returns>Representación de la unidad como string.</returns>
        public static string GetString(this Unidad unidad, bool singular)
            => (unidad, singular) switch
        {
            // Manejar casos de pluralidad especiales.
            (Unidad.UNIDAD, false) => "unidades",
            (_, true) => unidad.ToString("G").ToLower(),
            (_, false) => unidad.ToString("G").ToLower() + "s",
        };
    }
    
    /// <summary>
    /// Tipo de ingrediente registrado.
    /// </summary>
    public record class Ingrediente
    {
        /// <summary>Nombre del tipo de ingrediente.</summary>
        [Required(ErrorMessage = "{0} no debe estar vacío")]
        public string nombre {get; init;}
        /// <summary>Descripción del tipo de ingrediente.</summary>
        public string descripcion {get; init;}
        /// <summary>Unidades con las que se mide el ingrediente.</summary>
        public Unidad unidad {get; init;}
    }

    /// <summary>
    /// Validador de si un IngredienteCantidad medido en unidades tiene
    /// una cantidad entera.
    /// </summary>
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

    /// <summary>
    /// Cantidad determinada para un ingrediente.
    /// </summary>
    [UnidadCantidadEntera(ErrorMessage = "cantidad debe ser especificado en unidades enteras")]
    public record class IngredienteCantidad
    {
        /// <summary>
        /// Tipo de ingrediente.
        /// </summary>
        public Ingrediente ingrediente {get; init;}
        /// <summary>
        /// Cantidad (medida en ingrediente.unidad) del ingrediente.
        /// </summary>
        [Range(0.0, Double.MaxValue, ErrorMessage = "{0} no puede ser negativo")]
        public double cantidad {get; init;}
    }

    /// <summary>
    /// Representación de la cantidad en existencia de un ingrediente en el inventario.
    /// </summary>
    public record class IngredienteEnStock: IngredienteCantidad
    {
        /// <summary>
        /// Fecha en la que se vence el ingrediente.
        /// </summary>
        [CustomValidation(typeof(IngredienteEnStock), "ValidarFechaFutura")]
        public DateTime fechaVencimiento {get; init;}

        /// <summary>
        /// Validador de si fechaVencimiento está en el futuro.
        /// </summary>
        public static ValidationResult ValidarFechaFutura(DateTime fecha)
            => fecha > DateTime.Today
             ? ValidationResult.Success
             : new ValidationResult($"{nameof(fecha)} debe de estar en el futuro");
    }
}
