using System;
using Xunit;
using Xunit.Abstractions;
using Isas_Pizza;
using Isas_Pizza.Persistence;
using Isas_Pizza.Persistence.EFModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Isas_PizzaTests.Persistence
{
    /// <summary>
    /// Pruebas para la capa de persistencia basada en Entity Framework.
    /// </summary>
    public class EFPersistenceLayerTests
    {
        private readonly ITestOutputHelper output;
        Ingrediente ingrediente = new Ingrediente 
        {
            nombre = "Tomate",
            descripcion = "Tomate Alino",
            unidad = Unidad.UNIDAD
        };


        public EFPersistenceLayerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// \test
        /// <summary>
        /// Verificar que la capa de persistencia lance error cuando
        /// se lance en una base de datos que no existe.
        /// </summary>
        [Fact]
        public void CreacionEFPersistenceLayerEnBaseDeDatosNoExistenteFalla()
        {
            Assert.Throws<PersistenceException>(
                () => new EFPersistenceLayer("notfound.db")
            );
        }

        /// \test
        /// <summary>
        /// Probar si EFIngrediente se parsea correctamente de Ingrediente
        /// </summary>
        [Fact]
        public void ParsearEFIngredienteCorrectamente()
        {
            EFIngrediente ingredienteDb = new EFIngrediente(ingrediente);
            Assert.Equal(ingrediente, ingredienteDb.Export());
        }
    }
};
