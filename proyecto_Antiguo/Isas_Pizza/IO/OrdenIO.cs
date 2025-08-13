using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class OrdenIO : IBlockingDisplayer<Orden>, IBlockingPrompter<Orden>
    {

        private readonly MenuGenericoIO _menuGenerico = new MenuGenericoIO();
        private static readonly Random _random = new Random();
        public Orden Ask(Orden? _)
        {
            // Asumo que `_` es la lista de productos disponibles
            if (!(_ is ICollection<Producto> productosDisponibles))
                throw new ArgumentException("El parámetro debe ser una colección de Producto");

            if (!productosDisponibles.Any())
                throw new InvalidOperationException("No hay productos disponibles para seleccionar");

            
            var opciones = productosDisponibles
                .Select(p => (
                    $"{p.nombre}",  // Solo mostramos el nombre del producto
                    p
                ))
                .ToList();

            var productoSeleccionado = _menuGenerico.SelectOne(opciones);

            
            int cantidad;
            while (true)
            {
                Console.Write($"Cantidad (unidades): ");
                if (int.TryParse(Console.ReadLine(), out cantidad) && cantidad > 0)
                    break;
                Console.WriteLine("Debe ser un entero positivo");
            }

            // Crear la orden con el producto seleccionado
            var nuevaOrden = new Orden
            {
                numeroOrden = _random.Next(1000, 9999), // se asigna aleatorio
                estado = EstadoOrden.ORDENADA,
                productosOrdenados = new List<(Producto producto, int cantidad)>
                {
                    (productoSeleccionado, cantidad)
                },
                ordenadaEn = DateTime.Now
            };

            // Validar la orden
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(nuevaOrden, new ValidationContext(nuevaOrden), validationResults, true))
            {
                throw new ValidationException(string.Join("\n", validationResults.Select(v => v.ErrorMessage)));
            }

            return nuevaOrden;
        }


        public void Display(ICollection<Orden> elements)
        {
            Console.WriteLine("\n=== HISTORIAL DE ÓRDENES ===");

            foreach (var orden in elements.OrderBy(o => o.ordenadaEn))
            {
                Console.WriteLine($"\nOrden realizada: {orden.ordenadaEn:g}");
                Console.WriteLine("Productos:");

                foreach (var (producto, cantidad) in orden.productosOrdenados)
                {
                    Console.WriteLine($"- {cantidad}x {producto.nombre}");
                }
            }

            Console.WriteLine(new string('=', 30));
        }

       
    }
}
