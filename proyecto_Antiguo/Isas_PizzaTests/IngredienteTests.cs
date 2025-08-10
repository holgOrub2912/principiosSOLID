using System;
using Xunit;
using Xunit.Abstractions;
using Isas_Pizza;
using System.ComponentModel.DataAnnotations;

namespace Isas_PizzaTests
{
    public class IngredienteTests 
    {
        private readonly ITestOutputHelper output;

        public IngredienteTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private readonly TimeSpan timediff = new TimeSpan(20, 0, 0, 0, 0);


        [Theory]
        [InlineData(Unidad.UNIDAD, true, "unidad")]
        [InlineData(Unidad.UNIDAD, false, "unidades")]
        [InlineData(Unidad.LIBRA, true, "libra")]
        [InlineData(Unidad.LIBRA, false, "libras")]
        [InlineData(Unidad.GRAMO, true, "gramo")]
        [InlineData(Unidad.GRAMO, false, "gramos")]
        [InlineData(Unidad.LITRO, true, "litro")]
        [InlineData(Unidad.LITRO, false, "litros")]
        public void UnidadesConvertidasCorrectamente(Unidad unidad,
                                                     bool singular,
                                                     string expected)
            => Assert.Equal(expected, unidad.GetString(singular));

        [Fact]
        public void ValidacionNombreVacioFalla()
        {
            Ingrediente ingrediente = new Ingrediente
            {
                nombre = "",
                descripcion = "Tomate alino",
                unidad = Isas_Pizza.Unidad.UNIDAD
            };

            output.WriteLine(ingrediente.ToString());

            List<ValidationResult> resultadosValidacion = new();
            bool isValid = Validator
                .TryValidateObject(ingrediente,
                                   new ValidationContext(ingrediente),
                                   resultadosValidacion);

            Assert.Equal(
                "nombre no debe estar vacío",
                resultadosValidacion[0].ErrorMessage
            );
            Assert.False(isValid);
        }

        [Fact]
        public void ValidacionNombreLlenoPasa()
        {
            Ingrediente ingrediente = new Ingrediente
            {
                nombre = "Tomate",
                descripcion = "",
                unidad = Isas_Pizza.Unidad.UNIDAD
            };

            output.WriteLine(ingrediente.ToString());

            List<ValidationResult> resultadosValidacion = new();
            bool isValid = Validator
                .TryValidateObject(ingrediente,
                                   new ValidationContext(ingrediente),
                                   resultadosValidacion);

            Assert.Empty(resultadosValidacion);
            Assert.True(isValid);
        }
        
        [Fact]
        public void ValidacionIngredienteCantidadNegativaFalla()
        {
            IngredienteCantidad ingredienteCantidad = new IngredienteCantidad{
                ingrediente = new Ingrediente
                    {
                        nombre = "Ajo",
                        descripcion = "Diente de ajo",
                        unidad = Unidad.UNIDAD,
                    },
                cantidad = -30.0
            };

            output.WriteLine(ingredienteCantidad.ToString());

            List<ValidationResult> resultadosValidacion = new();
            bool isValid = Validator
                .TryValidateObject(ingredienteCantidad,
                                   new ValidationContext(ingredienteCantidad),
                                   resultadosValidacion, true);

            Assert.Equal(
                "cantidad no puede ser negativo",
                resultadosValidacion[0].ErrorMessage
            );
            Assert.False(isValid);
        }

        [Fact]
        public void ValidacionIngredienteCantidadPositivaPasa()
        {
            IngredienteCantidad ingredienteCantidad = new IngredienteCantidad
            {
                ingrediente = new Ingrediente
                    {
                        nombre = "Ajo",
                        descripcion = "Diente de ajo",
                        unidad = Unidad.UNIDAD,
                    },
                cantidad = 20.0
            };

            output.WriteLine(ingredienteCantidad.ToString());

            List<ValidationResult> resultadosValidacion = new();
            bool isValid = Validator
                .TryValidateObject(ingredienteCantidad,
                                   new ValidationContext(ingredienteCantidad),
                                   resultadosValidacion, true);

            Assert.Empty(resultadosValidacion);
            Assert.True(isValid);
        }

        [Fact]
        public void ValidacionIngredienteUnidadesDecimalesFallan()
        {
            IngredienteCantidad ingredienteCantidad = new IngredienteCantidad
            {
                ingrediente = new Ingrediente
                    {
                        nombre = "Ajo",
                        descripcion = "Diente de ajo",
                        unidad = Unidad.UNIDAD,
                    },
                cantidad = 34.2
            };

            output.WriteLine(ingredienteCantidad.ToString());

            List<ValidationResult> resultadosValidacion = new();
            bool isValid = Validator
                .TryValidateObject(ingredienteCantidad,
                                   new ValidationContext(ingredienteCantidad),
                                   resultadosValidacion, true);

            Assert.Equal(
                "cantidad debe ser especificado en unidades enteras",
                resultadosValidacion[0].ErrorMessage
            );
            Assert.False(isValid);
        }
        
        [Fact]
        public void ValidacionFechaVencimientoPasadaFalla()
        {
            IngredienteEnStock ingEnStock = new IngredienteEnStock
            {
                ingrediente = new Ingrediente
                    {
                        nombre = "Ajo",
                        descripcion = "Diente de ajo",
                        unidad = Unidad.UNIDAD,
                    },
                cantidad = 5.0,
                fechaVencimiento = DateTime.Today.Subtract(timediff),
            };

            output.WriteLine(ingEnStock.ToString());

            List<ValidationResult> resultadosValidacion = new();
            bool isValid = Validator
                .TryValidateObject(ingEnStock,
                                   new ValidationContext(ingEnStock),
                                   resultadosValidacion, true);

            Assert.Equal(
                "fecha debe de estar en el futuro",
                resultadosValidacion[0].ErrorMessage
            );
            Assert.False(isValid);
        }
    }
}
