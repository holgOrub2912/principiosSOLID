using System;
using Xunit;
using Xunit.Abstractions;
using Isas_Pizza;
using Isas_Pizza.Persistence;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

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
            EFPersistenceLayer efpl = new EFPersistenceLayer(new Dictionary<string,string>{
                {"Host", DefaultParameters.dbServer},
                {"Database", DefaultParameters.dbName},
                {"Username", DefaultParameters.dbUser},
                {"Password", DefaultParameters.dbPassword},
            });
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
        /// Probar el manejo de ingredientes en Stock
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

        /// \test
        /// <summary>
        /// Probar si es posible recuperar los productos de cada
        /// IPersistenceLayer
        /// </summary>
        [Fact]
        public void RecuperarProductos()
        {
            IROPersistenceLayer<Producto> productoPL = dbReset();
            IEnumerable<Producto> productos = productoPL.View((Producto?)null);
            Assert.Equal(InitData.productos.Count(),
                productos.Count()
            );
            Assert.Equal(InitData.productos[0].Nombre, productos.First().nombre);
        }

        /// \test
        /// <summary>
        /// Probar el manejo de órdenes
        /// </summary>
        [Fact]
        public void SalvarOrden()
        {
            EFPersistenceLayer efpl = dbReset();
            IPersistenceLayer<Orden> ordenPL = efpl;
            Producto producto = efpl.View((Producto?) null).First();

            Assert.Equal(0, ordenPL.View((Orden?) null).Count());
            // Guardado
            ordenPL.Save(
                new Orden[]{ new Orden {
                    productosOrdenados = new (Producto,int)[]{(producto,2)},
                    estado = EstadoOrden.ORDENADA,
                }}
            );

            // Lecutra
            IEnumerable<Orden> ordenes = ordenPL.View((Orden?)null);
            Orden orden = ordenes.First();
            Assert.Equal(1, ordenes.Count());
            Assert.Equal(2, orden.productosOrdenados.First().cantidad);
            Assert.Equal("Napolitana", orden.productosOrdenados.First().producto.nombre);

            // Actualización
            ordenPL.Update(
                ordenes.First(), new Orden {
                    numeroOrden = orden.numeroOrden,
                    estado = EstadoOrden.COCINANDO,
                    productosOrdenados = orden.productosOrdenados,
                    ordenadaEn = orden.ordenadaEn
                }
            );
            Assert.Equal(EstadoOrden.COCINANDO,
                ordenPL.View((Orden?) null).First().estado
            );

            // Eliminación
            ordenPL.Delete(orden);
            Assert.Equal(0, ordenPL.View((Orden?) null).Count());
        }
    }
}
