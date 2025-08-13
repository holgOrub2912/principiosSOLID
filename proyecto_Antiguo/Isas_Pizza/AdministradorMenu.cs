namespace Isas_Pizza
{
    public class AdministradorMenu : UserMenu
    {
        [MenuOption("Ver inventario")]
        public static void VerInventario(Pizzeria pizzeria)
        {
            pizzeria.ingredienteDp.Display(
                pizzeria.inventario.View(null).ToArray()
            );
        }
    }
}