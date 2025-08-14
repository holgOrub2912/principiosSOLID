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
            pizzeria.ordenes.Save([
                pizzeria.ordenPt.Ask(null)
            ]);
        }
    }
}