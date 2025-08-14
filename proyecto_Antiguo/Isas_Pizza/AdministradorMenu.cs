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

        [MenuOption("Agregar a inventario")]
        public static void AgregarAStock(Pizzeria pizzeria)
        {
            IngredienteEnStock toAdd = pizzeria.ingredientePt.Ask(null);
            IngredienteEnStock? target = pizzeria.inventario
                .View(null)
                .ToArray()
                .FirstOrDefault(i => i.ingrediente == toAdd.ingrediente);
            if (target is not null)
                pizzeria.inventario.Update(target, new IngredienteEnStock
                {
                    ingrediente = toAdd.ingrediente,
                    cantidad = toAdd.cantidad + target.cantidad,
                    fechaVencimiento = target.fechaVencimiento
                });
            else
                pizzeria.inventario.Save([toAdd]);
        }
    }
}