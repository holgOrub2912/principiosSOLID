using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using Isas_Pizza.Models;

namespace Isas_Pizza.UserMenus
{
    public class AdministradorMenu : UserMenu
    {
        [MenuOption("Ver inventario")]
        public static void VerInventario(Pizzeria pizzeria)
        {
            pizzeria.io.Display(
                pizzeria.inventario.View(null).ToArray()
            );
        }

        [MenuOption("Agregar a inventario")]
        public static void AgregarAStock(Pizzeria pizzeria)
        {
            Ingrediente ingrediente = pizzeria.io.SelectOne(
                "Seleccione el ingrediente que quiere agregar:",
                pizzeria.ingredientes
                    .View(null)
                    .Select(i => (i.nombre, i))
                    .ToArray()
            );

            IngredienteEnStock? toUpdate = pizzeria.inventario.View(null)
                .FirstOrDefault(ies =>
                    ies.ingrediente.nombre == ingrediente.nombre
                );
            
            if (toUpdate is not null)
            {
                pizzeria.inventario.Update(
                    toUpdate,
                    pizzeria.io.Ask(toUpdate)
                );
            }
            else
                pizzeria.inventario.Save([pizzeria.io.Ask(ingrediente)]);
        }

        [MenuOption("Agregar producto")]
        public static void AgregarProducto(Pizzeria pizzeria)
        {
            Producto producto = pizzeria.io.AskProducto();
            pizzeria.menu.Save([producto]);
        }
    }
}