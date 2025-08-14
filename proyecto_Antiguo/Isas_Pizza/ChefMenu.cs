namespace Isas_Pizza
{
    public class ChefMenu : UserMenu
    {
        [MenuOption("Ver Órdenes")]
        public static void VerOrdenes(Pizzeria pizzeria)
        {
            pizzeria.ordenDp.Display(
                pizzeria.ordenes.View(null).ToArray()
            );
        }

        [MenuOption("Cocinar una órden")]
        public static void CocinarOrden(Pizzeria pizzeria)
        {
            Orden ordenACocinar = pizzeria.selector.SelectOne(
                pizzeria.ordenes.View(null).Select(o =>
                    (o.numeroOrden.ToString() + " - " +
                     string.Join(", ", o.productosOrdenados
                        .Select(t => t.producto.nombre)
                     ), o)).ToArray()
            );
        }
    }
}