using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isas_Pizza.Persistence;
using Isas_Pizza.Models;

namespace Isas_Pizza.IO
{
    public class OrdenIO : IBlockingDisplayer<Orden>, IBlockingPrompter<Orden>
    {

        private readonly IBlockingSelector _menuGenerico;
        private readonly Func<IEnumerable<Producto>> _productGenerator;

        public OrdenIO(
            IBlockingSelector menuGenerico,
            Func<IEnumerable<Producto>> productGenerator
        )
        {
            this._menuGenerico = menuGenerico;
            this._productGenerator = productGenerator;
        }
        public Orden Ask(Orden? _)
        {
            IEnumerable<Producto> productos = _productGenerator();
            if (!productos.Any()){
                Console.WriteLine("Ups, no tenemos ninguna opción de nuestro menú disponible. Inténtalo más tarde.");
                return new Orden();
            }
            
            var opciones = productos
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
                Console.WriteLine("Erroren la orden: " + string.Join("\n", validationResults.Select(v => v.ErrorMessage)));
                return new Orden();
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
