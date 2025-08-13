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

        private readonly IBlockingSelector _menuGenerico;
        private readonly IEnumerable<Producto> _productos;
        private static readonly Random _random = new Random();

        public OrdenIO
        (
            IBlockingSelector menuGenerico,
            IEnumerable<Producto> productos
        )
        {
            this._menuGenerico = menuGenerico;
            this._productos = productos;
        }
        public Orden Ask(Orden? _)
        {
            if (!this._productos.Any())
                throw new InvalidOperationException("No hay productos disponibles para seleccionar");
            
            var opciones = this._productos
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
                // numeroOrden = _random.Next(1000, 9999), // se asigna aleatorio
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
