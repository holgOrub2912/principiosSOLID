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
    /// Pruebas para la capa de persistencia de autenticación
    /// </summary>
    public class AuthPersistenceLayerTests
    {
        private readonly ITestOutputHelper output;

        public AuthPersistenceLayerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// \test
        /// <summary>
        /// Verificar que la capa de persistencia lance error cuando
        /// se lance en una base de datos que no existe.
        /// </summary>
        [Fact]
        public void CreacionAuthPersistenceLayerEnBaseDeDatosNoExistenteFalla()
        {
            Assert.Throws<PersistenceException>(
                () => new AuthPersistenceLayer("notfound.txt")
            );
        }

    }
}