using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Isas_Pizza.Models;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class ProductoIO(
        IEnumerable<Ingrediente> ingredientes,
        PrimitiveIO primitiveIO,
        IBlockingSelector menuGenerico,
        Func<IEnumerable<Producto>> productosGen
    ): IBlockingDisplayer<Producto>
    {
        

        public void Display(ICollection<Producto> elements)
        {
            if (elements.Count == 0){
                Console.WriteLine("\nNo hay productos para mostrar");
                return;
            }


            Console.WriteLine("\n=== MENÚ DE PRODUCTOS ===");

            int maxNameLength = elements.Max(p => p.nombre.Length);
            // colección de IngredienteCantidad
            foreach (var producto in elements)
            {
                var ingredientes = producto.ingredientesRequeridos
                    .Select(i => $"- {i.ingrediente.nombre}");

                Console.WriteLine(
                    String.Format(
                        "{0,-" + maxNameLength.ToString() +"} {1,10}",
                        producto.nombre, producto.precio
                    ) +
                    $"\nIngredientes: {string.Join("\n", ingredientes)}"
                );
            }

            Console.WriteLine(new string('=', 25));
        }

        public Producto Ask(Producto? _)
        {
            IEnumerable<Producto> productos = productosGen();
            Console.Write("Nombre del producto: ");
            string nombre = primitiveIO.Ask("");

            // Verificar que este producto no exista ya
            while (productos.Any(p => p.nombre == nombre))
            {
                Console.WriteLine("Ya hay un producto con ese nombre.");
                Console.Write("Nombre del producto: ");
                nombre = primitiveIO.Ask("");
            }

            Console.Write("Precio del producto: ");
            double precio = primitiveIO.Ask(0.0);

            double cantidad;
            bool continuar = true;
            Ingrediente ingrediente;
            IEnumerable<IngredienteCantidad> ingredienteCantidades = [];
            while (continuar)
            {
                ingrediente = menuGenerico.SelectOne(
                    "Seleccione un ingrediente para el producto",
                    ingredientes.Select(i => (i.nombre, i)).ToArray()
                );
                Console.Write($"Cantidad ({ingrediente.unidad.GetString(false)}): ");
                cantidad = primitiveIO.Ask(0.0);
                ingredienteCantidades = ingredienteCantidades.Append(
                    new IngredienteCantidad {
                        ingrediente = ingrediente,
                        cantidad = cantidad
                });
                continuar = menuGenerico.SelectOne("Añadir más ingredientes?", 
                    [("Sí", true), ("No", false)]
                );
            }

            Producto producto = new Producto
            {
                nombre = nombre,
                precio = precio,
                ingredientesRequeridos = ingredienteCantidades.ToArray(),
            };

            List<ValidationResult> resultadosValidacion = [];
            if (!Validator
                .TryValidateObject(producto,
                                   new ValidationContext(producto),
                                   resultadosValidacion,
                                   true)
            ){
                Console.WriteLine("Parámetros de producto inválidos, inténtalo de nuevo.");
                return Ask(null);
            }
            return producto;
        }
    }
}