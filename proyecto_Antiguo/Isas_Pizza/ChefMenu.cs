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
    }
}