using Isas_Pizza.Persistence;

namespace Isas_Pizza
{
    public class MenuFilter(
        IPersistenceLayer<Producto> menu,
        IPersistenceLayer<IngredienteEnStock> inventario
    ) : IPersistenceLayer<Producto>
    {
        public void Delete(Producto producto) => menu.Delete(producto);

        public void Save(IEnumerable<Producto> productos) => menu.Save(productos);

        public void Update(Producto from, Producto to) => menu.Update(from, to);

        /// <summary>
        /// Filtra los productos en el inventario en base a si hay
        /// suficientes ingredientes en stock para preparar uno de ellos.
        /// </summary>
        /// <param name="_"></param>
        /// <returns>IEnumerable de productos que pueden ser preparados.</returns>
        public IEnumerable<Producto> View(Producto? _)
        {
            List<Producto> result = [];
            foreach (var producto in menu.View(null))
            {
                ICounterVisitor<bool, Producto> checker =
                    new InventarioChecker(
                        (IROPersistenceLayer<IngredienteEnStock>) menu);
                
                if (checker.Visit(producto))
                    result.Add(producto);
            }

            return result;
        }
    }
}