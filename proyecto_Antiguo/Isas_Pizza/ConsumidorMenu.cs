namespace Isas_Pizza
{
    public class ConsumidorMenu : UserMenu
    {
        [MenuOption("Ver Menú")]
        public static void VerMenu(Pizzeria pizzeria)
        {
            pizzeria.productoDp.Display(
                pizzeria.menu.View(null).ToList()
            );
        }

        [MenuOption("Ordenar")]
        public static void Ordenar(Pizzeria pizzeria)
        {
            Orden orden = pizzeria.ordenPt.Ask(null);
            if (orden.productosOrdenados.Count == 0)
                return;
            pizzeria.ordenes.Save([orden]);
        }
    }
}