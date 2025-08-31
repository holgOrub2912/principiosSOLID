namespace Isas_Pizza
{
    public static class OrderManager
    {
        public static void UpdateState(Pizzeria pizzeria, EstadoOrden from, EstadoOrden to)
        {
            Orden? toUpdate = GetWithState(pizzeria, from);
            if (toUpdate is null)
                return;

            pizzeria.ordenes.Update(toUpdate, new Orden {
                numeroOrden = toUpdate.numeroOrden,
                productosOrdenados = toUpdate.productosOrdenados,
                estado = to,
                ordenadaEn = toUpdate.ordenadaEn
            });
        }
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
    }
}