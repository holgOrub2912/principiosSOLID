using System;
using Xunit;
using Xunit.Abstractions;
using Isas_Pizza;
using System.ComponentModel.DataAnnotations;

namespace Isas_PizzaTests
{
    /// <summary>
    /// Pruebas para clases relacionadas con Orden
    /// </summary>
    public class OrdenTests 
    {
        private readonly ITestOutputHelper output;

        private static Producto productoEjemplo = new Producto
        {
            nombre = "Hawaiana",
            ingredientesRequeridos = new IngredienteCantidad[]{}
        };


        public OrdenTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// \test
        /// <summary>
        /// Probar que cantidades nulas o negativas no pueden ser insertadas
        /// en productosOrdenados
        /// </summary>
        [Fact]
        public void ValidacionCantidadNulaONegativaFalla()
        {
            Orden orden = new Orden
            {
                productosOrdenados = new (Producto producto, int cantidad)[]{
                    (productoEjemplo, -1),
                    (productoEjemplo, 0)
                }
            };

            Assert.False(orden.productosOrdenados.All(t => t.cantidad > 0));

            output.WriteLine(orden.ToString());
            output.WriteLine(orden.productosOrdenados.First().cantidad.ToString());

            List<ValidationResult> resultadosValidacion = new();
            bool isValid = Validator
                .TryValidateObject(orden,
                                   new ValidationContext(orden),
                                   resultadosValidacion, true);

            Assert.Equal(
                "productosOrdenados debe tener cantidades positivas",
                resultadosValidacion[0].ErrorMessage
            );
            Assert.False(isValid);
        }

        /// \test
        /// <summary>
        /// Probar que cantidades postivas pueden ser insertadas en productosOrdenados
        /// </summary>
        [Fact]
        public void ValidacionCantidadPositivaPasa()
        {
            Orden orden = new Orden
            {
                productosOrdenados = new (Producto producto, int cantidad)[]{
                    (productoEjemplo, 3),
                    (productoEjemplo, 2)
                }
            };

            Assert.True(orden.productosOrdenados.All(t => t.cantidad > 0));

            output.WriteLine(orden.ToString());
            output.WriteLine(orden.productosOrdenados.First().cantidad.ToString());

            List<ValidationResult> resultadosValidacion = new();
            bool isValid = Validator
                .TryValidateObject(orden,
                                   new ValidationContext(orden),
                                   resultadosValidacion, true);

            Assert.Empty(resultadosValidacion);
            Assert.True(isValid);
        }
    }
}
