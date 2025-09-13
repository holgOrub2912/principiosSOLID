using System.Drawing;
using Isas_Pizza.Models;

namespace Isas_Pizza.UserMenus
{
    public class ChefMenu : UserMenu
    {
        [MenuOption("Ver Órdenes")]
        public static void VerOrdenes(Pizzeria pizzeria)
        {
            pizzeria.io.Display(
                pizzeria.ordenes.View(null).ToArray()
            );
        }

        [MenuOption("Cocinar una órden")]
        public static void CocinarOrden(Pizzeria pizzeria)
        {
            InventarioUpdater updater = new(pizzeria.inventario);
            updater.SetNext(
                new OrdenUpdater(pizzeria.ordenes,
                                 EstadoOrden.COCINANDO)
            );

            try  {
                OrderManager.HandleWithState(pizzeria, EstadoOrden.ORDENADA, updater);
            } catch (InventarioInsuficienteException e)
            {
                pizzeria.io.Display([$"Inventario insuficiente: {e.nombreIngrediente}"]);
                return;
            }
            pizzeria.io.Display(["Cocinando ..."]);
        }

        [MenuOption("Despachar orden")]
        public static void DespacharOrden(Pizzeria pizzeria)
            => OrderManager.UpdateState(pizzeria,
                                        EstadoOrden.COCINANDO,
                                        EstadoOrden.LISTA);
    }
}