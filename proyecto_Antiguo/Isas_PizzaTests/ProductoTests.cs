using System;
using Xunit;
using Xunit.Abstractions;
using Isas_Pizza;
using System.ComponentModel.DataAnnotations;

namespace Isas_PizzaTests
{
    /// <summary>
    /// Pruebas para clases relacionadas con Producto
    /// </summary>
    public class ProductoTests
    {
        private readonly ITestOutputHelper output;

        public ProductoTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// \test
        /// <summary>
        /// Probar que un nombre vacío dispare un error de validación.
        /// </summary>
        [Fact]
        public void ValidacionNombreVacioFalla()
        {
            Ingrediente ingrediente = new Ingrediente
            {
                nombre = "Tomate",
                descripcion = "Tomate alino",
                unidad = Isas_Pizza.Unidad.UNIDAD
            };

            Producto producto = new Producto
            {
                nombre = "",
                ingredientesRequeridos = new IngredienteCantidad[]
                {
                    new IngredienteCantidad
                    {
                        ingrediente = ingrediente,
                        cantidad = 9.0
                    }
                }
            };

            output.WriteLine(producto.ToString());

            List<ValidationResult> resultadosValidacion = new();
            bool isValid = Validator
                .TryValidateObject(producto,
                                   new ValidationContext(producto),
                                   resultadosValidacion, true);

            Assert.Equal(
                "nombre no debe estar vacío",
                resultadosValidacion[0].ErrorMessage
            );
            Assert.False(isValid);
        }

        /// \test
        /// <summary>
        /// Probar que un nombre no vacío pasa la validación.
        /// </summary>
        [Fact]
        public void ValidacionNombreLlenoPasa()
        {
            Ingrediente ingrediente = new Ingrediente
            {
                nombre = "Tomate",
                descripcion = "Tomate alino",
                unidad = Isas_Pizza.Unidad.UNIDAD
            };

            Producto producto = new Producto
            {
                nombre = "Pizza napolitana",
                ingredientesRequeridos = new IngredienteCantidad[]
                {
                    new IngredienteCantidad
                    {
                        ingrediente = ingrediente,
                        cantidad = 9.0
                    }
                }
            };

            output.WriteLine(producto.ToString());

            List<ValidationResult> resultadosValidacion = new();
            bool isValid = Validator
                .TryValidateObject(producto,
                                   new ValidationContext(producto),
                                   resultadosValidacion, true);

            Assert.Empty(resultadosValidacion);
            Assert.True(isValid);
        }
    }
}
