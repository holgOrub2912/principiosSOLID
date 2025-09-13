using Isas_Pizza.Models;
using Isas_Pizza.Persistence;

namespace Isas_Pizza {
    public class OrdenUpdater (
        IPersistenceLayer<Orden> ordenes,
        EstadoOrden targetState
    ) : IRequestHandler<Orden>
    {
        IRequestHandler<Orden>? _next;
        public void Handle(Orden orden)
        {
            ordenes.Update(orden, new Orden
            {
                numeroOrden = orden.numeroOrden,
                productosOrdenados = orden.productosOrdenados,
                ordenadaEn = orden.ordenadaEn,
                estado = targetState
            });

            _next?.Handle(orden);
        }

        public void SetNext(IRequestHandler<Orden> next)
        {
            this._next = next;
        }
    }
}