using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class ProductoIO(
        IEnumerable<Ingrediente> ingredientes,
        PrimitiveIO primitiveIO,
        IBlockingSelector menuGenerico
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
            Console.Write("Nombre del producto: ");
            string nombre;
            do
                nombre = Console.ReadLine();
            while (string.IsNullOrEmpty(nombre));
            double precio;
            do {
                Console.Write("Precio del producto: ");
                precio = primitiveIO.Ask(0.0);
            } while (precio <= 0.0);

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
                while (!double.TryParse(Console.ReadLine(), out cantidad) || cantidad <= 0)
                    Console.Write("Debe ser un entero positivo\nCantidad: ");
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

            List<ValidationResult> resultadosValidacion = new();
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