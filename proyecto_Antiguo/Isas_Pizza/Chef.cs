using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza
{
    //suscriber de orden lista
    public class Chef
    {
        public void EventHandler_OrdenLista(object sender, CustomArgs c)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Cocinando ...");
            Thread.Sleep(c.Orden.Duracion);

            Console.ForegroundColor = ConsoleColor.Cyan;
            //lo que sucede cuando pasa el evento
            Console.WriteLine($"La Orden {c.Orden.NombreOrden} esta lista ");

            Ingrediente ingEliminar = c.Orden.IngredienteO;
           // Console.WriteLine(ingEliminar.Imprimirse());

            Console.ResetColor();
            
            
           c.Inventario.EliminarIngChef(ingEliminar);
            Console.WriteLine("\nEl Inventario quedo asi\n");
           c.Inventario.VerIng();
            
           
            

            
        }
    }

}
