using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

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
            pizzeria.stringDp.Display(["Seleccione el ingrediente que quiere agregar:"]);
            Ingrediente ingrediente = pizzeria.selector.SelectOne<Ingrediente>(
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
                pizzeria.stringDp.Display([$"¿Qué cantidad desea agregar? ({ingrediente.unidad.GetString(false)})"]);
                double cantidad = ingrediente.unidad == Unidad.UNIDAD
                    ? pizzeria.intPt.Ask(0)
                    : pizzeria.doublePt.Ask(0.0);

                pizzeria.inventario.Update(
                    toUpdate,
                    new IngredienteEnStock {
                        ingrediente = ingrediente,
                        cantidad = toUpdate.cantidad + cantidad,
                        fechaVencimiento = toUpdate.fechaVencimiento
                    }
                );
            }
            else
                pizzeria.inventario.Save([pizzeria.ingredientePt.Ask(ingrediente)]);
        }
    }
}