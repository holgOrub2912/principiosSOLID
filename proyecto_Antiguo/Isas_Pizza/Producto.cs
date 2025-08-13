using System;
using System.ComponentModel.DataAnnotations;

namespace Isas_Pizza
{
    /**
    <summary>
    Producto que puede ser ordenado por un Consumidor y estar en el Menú.
    </summary>
    */
    public record class Producto
    {
        [Required(ErrorMessage = "{0} no debe estar vacío")]
        /// <summary>Nombre del producto.</summary>
        public string nombre {get; init;}
        [Required(ErrorMessage = "{0} no debe estar vacío")]
        [Range(0.0, Double.MaxValue)]
        public double precio {get; init;}

        /// <summary>Arreglo de los ingredientes y cantidades que requiere el producto para ser cocinado.</summary>
        public ICollection<IngredienteCantidad> ingredientesRequeridos {get; init;}
    }
}
