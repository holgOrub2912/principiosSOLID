namespace Isas_Pizza
{
    public static class OrderManager
    {
        public static void UpdateState(Pizzeria pizzeria, EstadoOrden from, EstadoOrden to)
        {
            IEnumerable<Orden> ordenesListas = pizzeria.ordenes
                    .View(null)
                    .ToList()
                    .FindAll(o => o.estado == from);

            if (!ordenesListas.Any()){
                pizzeria.stringDp.Display(["No hay órdenes disponibles."]);
                return;
            }

            Orden toUpdate = pizzeria.selector.SelectOne(ordenesListas
                .Select(o => (string.Join(", ", o.productosOrdenados
                    .Select(t => $"{t.producto.nombre} x{t.cantidad}")
                    ), o))
                .ToList()
            );

            pizzeria.ordenes.Update(toUpdate, new Orden {
                numeroOrden = toUpdate.numeroOrden,
                productosOrdenados = toUpdate.productosOrdenados,
                estado = to,
                ordenadaEn = toUpdate.ordenadaEn
            });
        }
    }
}