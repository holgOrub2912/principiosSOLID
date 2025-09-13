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
            Orden? posibleOrden = OrderManager.GetWithState(pizzeria, EstadoOrden.ORDENADA);
            if (posibleOrden is null)
                return;
            Orden ordenACocinar = posibleOrden;

            InventarioUpdater updater = new(pizzeria.inventario);
            try  {
                updater.ApplyUpdate(ordenACocinar);
            } catch (InventarioInsuficienteException e)
            {
                pizzeria.io.Display([$"Inventario insuficiente: {e.nombreIngrediente}"]);
                return;
            }

            // Marcar la orden como cocinándose
            pizzeria.ordenes.Update(ordenACocinar, new Orden
            {
                numeroOrden = ordenACocinar.numeroOrden,
                productosOrdenados = ordenACocinar.productosOrdenados,
                estado = EstadoOrden.COCINANDO,
                ordenadaEn = ordenACocinar.ordenadaEn
            });

            pizzeria.io.Display(["Cocinando ..."]);
        }

        [MenuOption("Despachar orden")]
        public static void DespacharOrden(Pizzeria pizzeria)
            => OrderManager.UpdateState(pizzeria,
                                        EstadoOrden.COCINANDO,
                                        EstadoOrden.LISTA);
    }
}