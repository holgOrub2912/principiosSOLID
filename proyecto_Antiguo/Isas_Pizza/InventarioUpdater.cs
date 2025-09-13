using Isas_Pizza.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using Isas_Pizza.Models;
using System.Linq;

namespace Isas_Pizza {
    public class InventarioUpdater
    (
        IPersistenceLayer<IngredienteEnStock> inventario
    ) : IRequestHandler<Orden>
    {
        private IRequestHandler<Orden>? _next;
        public void Handle(Orden orden)
        {
            IEnumerable<IngredienteEnStock> inventarioViejo = inventario
                .View(null);
            Dictionary<string, double> inventarioActualizado = inventarioViejo
                .ToDictionary(
                    ies => ies.ingrediente.nombre,
                    ies => ies.cantidad
                );

            ICounterVisitor<bool, Orden> checker = new InventarioChecker(inventarioActualizado);
            if (!checker.Visit(orden))
                throw new InventarioInsuficienteException(inventarioActualizado.Single(t => t.Value < 0).Key);

            inventarioActualizado.Join(
                inventarioViejo,
                t => t.Key,
                ies => ies.ingrediente.nombre,
                (actualizado, viejo) => (viejo,
                new IngredienteEnStock
                {
                    ingrediente = viejo.ingrediente,
                    cantidad = actualizado.Value,
                    fechaVencimiento = viejo.fechaVencimiento
                })
            ).ToImmutableList().ForEach(
                ((IngredienteEnStock viejo,
                    IngredienteEnStock nuevo) item)
                => inventario.Update(item.viejo, item.nuevo));

            _next?.Handle(orden);
        }

        public void SetNext(IRequestHandler<Orden> next)
        {
            this._next = next;
        }
    }
}