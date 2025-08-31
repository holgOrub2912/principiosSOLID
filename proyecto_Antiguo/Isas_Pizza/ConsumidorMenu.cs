using System.Drawing;

namespace Isas_Pizza
{
    public class ConsumidorMenu : UserMenu
    {
        [MenuOption("Ver Menú")]
        public static void VerMenu(Pizzeria pizzeria)
        {
            pizzeria.io.Display(
                pizzeria.menu.View(null).ToList()
            );
        }

        [MenuOption("Ordenar")]
        public static void Ordenar(Pizzeria pizzeria)
        {
            Orden orden = pizzeria.io.AskOrden();
            if (orden.productosOrdenados is null)
                return;

            pizzeria.ordenes.Save([orden]);
        }

        [MenuOption("Recibir orden")]
        public static void Recibir(Pizzeria pizzeria)
            => OrderManager.UpdateState(pizzeria,
                                        EstadoOrden.LISTA,
                                        EstadoOrden.ENTREGADA);
    }
}