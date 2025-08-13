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
    }
}