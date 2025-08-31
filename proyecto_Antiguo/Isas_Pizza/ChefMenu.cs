using System.Drawing;

namespace Isas_Pizza
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

            Dictionary<string, IngredienteEnStock> invArr
                = pizzeria.inventario.View(null).ToDictionary(ies => ies.ingrediente.nombre);
            List<(IngredienteEnStock, IngredienteEnStock)> toUpdate = new();

            // Quitar del inventario los ingredientes empleados
            double resultantQty;
            IngredienteEnStock ies;
            foreach ((Producto producto, int prodCant) in ordenACocinar.productosOrdenados)
                foreach (IngredienteCantidad ingCant in producto.ingredientesRequeridos){

                    // Verificar que si haya suficiente de este ingrediente
                    resultantQty = (ies = invArr.GetValueOrDefault(ingCant.ingrediente.nombre)).cantidad
                        - ingCant.cantidad * prodCant;
                        
                    if (resultantQty < 0){
                        pizzeria.io.Display([
                            $"No es posible cocinar esta orden - hace falta {ingCant.ingrediente.nombre}"
                        ]);
                        return;
                    }

                    toUpdate.Add((ies, new IngredienteEnStock
                    {
                        ingrediente = ies.ingrediente,
                        cantidad = resultantQty,
                        fechaVencimiento = ies.fechaVencimiento,
                    }));
                }

            toUpdate.ForEach(t =>
                pizzeria.inventario.Update(t.Item1, t.Item2)
            );

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