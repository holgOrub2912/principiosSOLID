using System;
using Xunit;
using Isas_Pizza;

namespace Isas_PizzaTests
{
    public class IngredienteTests 
    {
        private readonly TimeSpan timediff = new TimeSpan(20, 0, 0, 0, 0);

        [Fact]
        public void NombreVacioFalla()
        {
            Assert.Throws<ArgumentException>("nombre",
                () => new Ingrediente("", new DateTime(1900, 3, 4), 2.0, "Libras de tomate.")
            );
        }

        [Fact]
        public void FechaVencimientoPasadaFalla()
        {
            Assert.Throws<ArgumentOutOfRangeException>("fechaVencimiento",
                () => new Ingrediente("Tomate", DateTime.Today.Subtract(timediff), 2.0, "Libras de tomate.")
            );
        }

        [Theory]
        [InlineData(-10.0)]
        [InlineData(0.0)]
        public void PesoInvalidoFalla(double val)
        {
            Assert.Throws<ArgumentOutOfRangeException>("peso",
                () => new Ingrediente("Tomate", DateTime.Today.Add(timediff), val, "Libras de tomate.")
            );
        }
    }
}
