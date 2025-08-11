using System;
using Xunit;
using Xunit.Abstractions;
using Isas_Pizza;
using Isas_Pizza.Persistence;
using System.ComponentModel.DataAnnotations;

namespace Isas_PizzaTests.Persistence
{
    /// <summary>
    /// Pruebas para las varias implementaciones de IPersisteceLayer
    /// e IROPersistenceLayer
    /// </summary>
    public class PersistenceLayerTests
    {
        private readonly ITestOutputHelper output;
        
        private static Ingrediente ingrediente = new Ingrediente 
        {
            nombre = "Tomate",
            descripcion = "Tomate Alino",
            unidad = Unidad.UNIDAD
        };

        private EFPersistenceLayer dbReset()
        {
            EFPersistenceLayer efpl = new EFPersistenceLayer();
            efpl._initData();
            return efpl;
        }

        private static IngredienteEnStock ingredienteES = new IngredienteEnStock
        {
            ingrediente = ingrediente,
            cantidad = 4.0,
            fechaVencimiento = DateTime.Today
        };

        public PersistenceLayerTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        
        /// \test
        /// <summary>
        /// Probar si es posible recuperar los ingredientes de cada
        /// IPersistenceLayer
        /// </summary>
        [Fact]
        public void RecuperarIngredientes()
        {
            IROPersistenceLayer<Ingrediente> ingredientePL = dbReset();
            Assert.Equal(InitData.ingredientes.Count(),
                ingredientePL.View((Ingrediente?) null).Count()
            );
        }

        /// \test
        /// <summary>
        /// Probar si es posible crear un nuevo ingredienteEnStock en
        /// una IPersistenceLayer
        /// </summary>
        [Fact]
        public void SalvarIngredienteEnStock()
        {
            EFPersistenceLayer efpl = dbReset();
            IPersistenceLayer<IngredienteEnStock> ingredienteESPL = efpl;
            Assert.Equal(0, ingredienteESPL.View((IngredienteEnStock?) null).Count());
            // Guardado
            ingredienteESPL.Save(
                new IngredienteEnStock[]{ingredienteES}
            );

            // Lecutra
            Assert.Equal(1, ingredienteESPL.View((IngredienteEnStock?) null).Count());

            // Actualización
            ingredienteESPL.Update(
                ingredienteES, new IngredienteEnStock {
                    ingrediente = ingredienteES.ingrediente,
                    cantidad = ingredienteES.cantidad - 1,
                    fechaVencimiento = ingredienteES.fechaVencimiento
                }
            );
            Assert.Equal(3, ingredienteESPL.View((IngredienteEnStock?) null).First().cantidad);

            // Eliminación
            ingredienteESPL.Delete(ingredienteES);
            Assert.Equal(0, ingredienteESPL.View((IngredienteEnStock?) null).Count());
        }

        /*
        /// \test
        /// <summary>
        /// Probar si es posible crear un nuevo ingredienteEnStock en
        /// una IPersistenceLayer
        /// </summary>
        [Fact]
        public void ActualizarIngredienteEnStock()
        {
            IPersistenceLayer<IngredienteEnStock> ingredienteESPL
                = new EFPersistenceLayer();
            Assert.Equal(1, ingredientePL.Save<Ingrediente>(
                    new IngredienteEnStock[]{ingredienteES}
            ));
        }
        */
    }
}
