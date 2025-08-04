using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza
{
    public class IDArgs
    {
        public string ID;
    }

    //Publisher de orden lista
    public class Administrador
    {
        public Inventario Inventario_;
        public Administrador(Inventario inventario)
        {
            Inventario_ = inventario;
        }

        //eventos
        public event EventHandler<CustomArgs> Orden_Lista;
        public event EventHandler<IDArgs> Sesion_Iniciada;

        //funciones que lanzan eventos
        protected virtual void On_OrdenLista(Orden ordenE, Inventario invTrabajar)
        {
            CustomArgs args = new CustomArgs(invTrabajar, ordenE);
            if(Orden_Lista != null) Orden_Lista?.Invoke(this, args);

        }
        protected virtual void On_SesionIniciada(string id)
        {

            if (Sesion_Iniciada != null) {
                Sesion_Iniciada?.Invoke(this, new IDArgs { ID = id });
            } 
        }

        //Metodo que desencadena
        public void EscogerMenu()
        {
            byte opcion; bool exito; String ID;
            bool salir = false;
            do
            {
                Console.WriteLine("\nDigite el numero del menu deseado: \n1.Cliente \t2.Trabajador\n0. Finalizar Operacion");
                exito = Byte.TryParse(Console.ReadLine(), out opcion);

                if (!exito) Console.WriteLine("Valor no valido");
                switch (opcion)
                {
                    case 1:
                        MostrarMenuCliente();
                        break;
                    case 2:
                        //id para entrar
                        Console.Write("Ingrese su ID: ");
                        ID = LeerEntrada();
                        if (ValidarEntrada(ID)) { MostrarMenuTrabajador(ID); }
                        else { Console.WriteLine("No se puede iniciar sesion"); }
                        break;
                    case 0:
                        Console.WriteLine("Adios!!");
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no valida, escoja un menu");
                        break;
                }

            } while (!salir);

        }

        private void MostrarMenuTrabajador(string ID)
        {
            On_SesionIniciada(ID);
            byte opcion; bool exito;
            bool salir = false;
            Console.WriteLine("Digite el numero de la acción a realizar");
            
            do
            {
                Console.WriteLine("0. Salir\n" +
                "1. Agregar\n" +
                "2. Eliminar\n" +
                "3. Contar\n" +
                "4. Filtar\n" +
                "5. Ver\n" +
                "6. Buscar\n" +
                "7. Obtener info Pesos\n" +
                "8. Ordenar\n" +
                "9. Exportar Datos\n");

                exito = Byte.TryParse(Console.ReadLine(), out opcion);
                switch (opcion)
                {
                    case 0:
                        salir = true;
                        break;
                    case 1:
                        Inventario_.AgregarIng();
                        break;
                    case 2:
                        Inventario_.EliminarIng();
                        break;
                    case 3:
                        Inventario_.ContarIng();
                        break;
                    case 4:
                        Inventario_.FiltrarIng();
                        break;
                    case 5:
                        Inventario_.VerIng();
                        break;
                    case 6:
                        Inventario_.BuscarIng();
                        break;
                    case 7:
                        Inventario_.InfoPesosIng();
                        break;
                    case 8:
                        Inventario_.OrdenarIng();
                        break;
                    case 9:
                        Inventario_.ExportarDatosInventario();
                        break;
                    default:
                        Console.WriteLine("Digite una opción valida");
                        break;

                }

            } while (!salir);
        }

        private void MostrarMenuCliente()
        {
            Console.WriteLine("Bienvenido a Nuestra increible Pizzaria!!\n" +
                "Somos una pizzeria unica puesto que aqui tu no eres quien elije que comer, somos nosotros!\n" +
                "Se te mostará el Menú a continuación y tu solo podrás ver un ingrediente de cada opción\n" +
                "Buen Provecho!!!\n");
            Thread.Sleep(2000);
          
            string opcion=""; int duracion=0;
            //debería meterlo en un ciclo??
            do {
                Console.WriteLine("\tMenu\n" +
                "1.Hogar ---> Platano\n" +
                "2.Paisita ---> Frijoles\n" +
                "3.Peperonni ---> Peperonni \n" +
                "4.Margarita ---> Tomate\n"+
                "5.Ranchera ---> Tocineta"
                );

                Console.WriteLine("\n¿Qué desea ordenar? (escriba) ");

            opcion = Console.ReadLine().ToLower();
            Ingrediente ingreEscogido; 
            Orden ordenEnviar;
            switch (opcion)
            {
                case "hogar":
                    ingreEscogido = new Ingrediente
                    {
                        Nombre = "Platano"
                    };
                        duracion = 3000;
                    break;
                case "paisita":
                    ingreEscogido = new Ingrediente
                    {
                        Nombre = "Frijoles"
                    };
                        duracion = 2000;
                        break;
                case "peperonni":
                    ingreEscogido = new Ingrediente
                    {
                        Nombre = "Peperonni"
                    };
                        duracion = 1000;
                        break;
                case "ranchera":
                    ingreEscogido = new Ingrediente
                    {
                        Nombre = "Tocineta"
                    };
                        duracion = 1500;
                        break;
                case "margarita":
                    ingreEscogido = new Ingrediente
                    {
                        Nombre = "Tomate"
                    };
                        duracion = 1000;
                        break;
                default:
                    Console.WriteLine("Escoja algo del menú");
                    ingreEscogido = null;
                    break;
            }

            //Invocar evento
            if(ingreEscogido != null && Inventario_.BuscarIngChef(ingreEscogido))
            {
                ordenEnviar = new Orden(ingreEscogido,opcion,duracion);
                On_OrdenLista(ordenEnviar, Inventario_);
            }
            else { Console.WriteLine("No ingredientes suficientes / Acabo orden"); }
           

          } while (opcion.ToLower() != "salir");

        }

        private bool ValidarEntrada(String ID)
        {
            bool cumpleRequisitos = ID.All(c => char.IsLetterOrDigit(c));
            if(cumpleRequisitos && ID.Length >=4 ) { return true; }
            else { return false; }
        }

        private string LeerEntrada()
        {
            string contra = "";
            ConsoleKeyInfo tecla;
            do
            {
                tecla = Console.ReadKey(true); //leer una tecla sin mostrarla en la consola

                if (tecla.Key != ConsoleKey.Backspace && tecla.Key != ConsoleKey.Enter)
                {
                    contra += tecla.KeyChar;
                    Console.Write("*"); // Muestra un asterisco en lugar del caracter
                }
                else if (tecla.Key == ConsoleKey.Backspace && contra.Length > 0)
                {
                    contra = contra.Remove(contra.Length - 1);
                    Console.Write("\b \b"); // Borra el último asterisco
                }
            } while (tecla.Key != ConsoleKey.Enter);
            Console.WriteLine(); // Salto de línea después de la entrada
            return contra;
        }

    }
}
