using Isas_Pizza.Models;

namespace Isas_Pizza
{
    public static class OrderManager
    {
        public static void UpdateState(Pizzeria pizzeria, EstadoOrden from, EstadoOrden to)
            => HandleWithState(pizzeria,
                                from,
                                new OrdenUpdater(pizzeria.ordenes, to)
                              );
        public static Orden? GetWithState(Pizzeria pizzeria, EstadoOrden targetState)
        {
            IEnumerable<Orden> ordenesListas = pizzeria.ordenes
                    .View(null)
                    .ToList()
                    .FindAll(o => o.estado == targetState);

            if (!ordenesListas.Any()){
                pizzeria.io.Display(["No hay órdenes disponibles."]);
                return null;
            }

            return pizzeria.io.SelectOne(ordenesListas
                .Select(o => (string.Join(", ", o.productosOrdenados
                    .Select(t => $"{t.producto.nombre} x{t.cantidad}")
                    ), o))
                .ToList()
            );
        }
        public static void HandleWithState(Pizzeria pizzeria, EstadoOrden state, IRequestHandler<Orden> handler)
        {
            Orden? orden = GetWithState(pizzeria, state);
            if (orden is null)
                return;

            handler.Handle(orden);
        }
    }
}