using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza
{
    public class Ingrediente
    {
        private DateTime _fechaVencimiento;

        public DateTime FechaVencimiento { 
            get => _fechaVencimiento;
            set
            {
                if (value >= DateTime.Today)
                {
                    _fechaVencimiento = value;
                }
                else
                {
                    _fechaVencimiento = DateTime.Today;
                }
            }
        }
        private String _nombre;

        public String Nombre { 
            get => _nombre;
            set {
                if (!string.IsNullOrEmpty(value))
                {
                    // Convierte la primera letra a mayúscula y el resto a minúscula
                    _nombre = char.ToUpper(value[0]) + value.Substring(1).ToLower();
                }
                else
                {
                    _nombre = "N/A";
                }
            }
        }
        
        public double Peso;
        public String Descripcion;

        public enum Estado
        {
            Principal,
            Secundario,
            Decorativo
        }

        public Estado Estado_;

        public Ingrediente()
        {
            Nombre = "";
            Peso = 1;
            Descripcion = "";
            FechaVencimiento= DateTime.Today;
            Estado_ = Estado.Secundario;
        }
       
       
        public string Imprimirse()
        {
            return "-> Nombre " + Nombre + " | Peso " + Peso + " | Estado " + Estado_+ " | Fecha Vencimiento "+ FechaVencimiento.ToString("dd MMMM yyyy") +
                "\nDescripcion: " + Descripcion;
        }
    }
}
