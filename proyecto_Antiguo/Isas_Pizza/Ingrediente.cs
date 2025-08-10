using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza
{
    public class Ingrediente
    {
        public string nombre {get;}
        public DateTime fechaVencimiento {get;}
        public double peso {get;}
        public string descripcion {get;}

        public static void ValidateArgs(string nombre,
                                        DateTime fechaVencimiento,
                                        double peso,
                                        string descripcion)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new ArgumentException(
                    "Nombre del ingrediente no puede ser nulo",
                    nameof(nombre)
                );
            if (fechaVencimiento < DateTime.Today)
                throw new ArgumentOutOfRangeException(
                    nameof(fechaVencimiento),
                    "La fecha de vencimiento no puede estar en el pasado"
                );
            if (peso <= 0.0)
                throw new ArgumentOutOfRangeException(
                    nameof(peso),
                    "El peso debe de ser un valor positivo"
                );
        }

        public Ingrediente(string nombre,
                           DateTime fechaVencimiento,
                           double peso,
                           string descripcion)
        {
            ValidateArgs(nombre, fechaVencimiento, peso, descripcion);
            this.nombre = nombre;
            this.fechaVencimiento = fechaVencimiento;
            this.peso = peso;
            this.descripcion = descripcion;
        }
    }
}
