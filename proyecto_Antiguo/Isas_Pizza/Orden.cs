using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza
{
    public class Orden
    {
        public Ingrediente IngredienteO { get; set; }
        public string NombreOrden;
        public int Duracion;
        public Orden(Ingrediente Ingrediente, string Nombre, int Duracion)
        {
            IngredienteO= Ingrediente;
            NombreOrden= Nombre;
            this.Duracion= Duracion;
        }
    }

    public class CustomArgs : EventArgs
    {
        public Inventario Inventario { get; set; }
        public Orden Orden { get; set; }

        public CustomArgs(Inventario inventario, Orden orden)
        {
            Inventario = inventario;
            Orden = orden;
        }
    }
}
