using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Isas_Pizza
{
    public class Inventario
    {
        private List<Ingrediente> Ingredientes;

        public Inventario()
        {
            Ingredientes= new List<Ingrediente>();

            Ingredientes.Add(new Ingrediente { Nombre = "Tomate", Peso=3 , Descripcion="Para salsa",
                Estado_=Ingrediente.Estado.Principal
            });
            Ingredientes.Add(new Ingrediente
            {
                Nombre = "Masa",
                Peso = 5.7,
                FechaVencimiento= new DateTime(2024, 6, 4),
                Descripcion = "Fermentado",
                Estado_ = Ingrediente.Estado.Principal
            });
            Ingredientes.Add(new Ingrediente
            {
                Nombre = "Tocineta",
                Peso = 2,
                FechaVencimiento = new DateTime(2024, 6, 3),
                Descripcion = "De cerdo",
                Estado_ = Ingrediente.Estado.Principal
            });
            Ingredientes.Add(new Ingrediente
            {
                Nombre = "Tocineta",
                Peso = 1.5,
                FechaVencimiento = new DateTime(2024, 6, 5),
                Descripcion = "De cerdo",
                Estado_ = Ingrediente.Estado.Principal
            });
            Ingredientes.Add(new Ingrediente
            {
                Nombre = "Peperonni",
                FechaVencimiento = new DateTime(2024, 8, 3),
                Peso = 0.4,
                Descripcion = "Topping",
                Estado_ = Ingrediente.Estado.Secundario
            });
            Ingredientes.Add(new Ingrediente
            {
                Nombre = "Peperonni",
                FechaVencimiento = new DateTime(2024, 8, 8),
                Peso = 0.5,
                Descripcion = "Topping",
                Estado_ = Ingrediente.Estado.Secundario
            });
            Ingredientes.Add(new Ingrediente
            {
                Nombre = "Tomate",
                FechaVencimiento = new DateTime(2024, 5, 12),
                Peso = 2,
                Descripcion = "Para salsa",
                Estado_ = Ingrediente.Estado.Principal
            });
            Ingredientes.Add(new Ingrediente
            {
                Nombre = "Frijoles",
                FechaVencimiento = new DateTime(2025, 1, 2),
                Peso = 0.9,
                Descripcion = "Un poco picantes",
                Estado_ = Ingrediente.Estado.Decorativo
            });
            Ingredientes.Add(new Ingrediente
            {
                Nombre = "Platano",
                FechaVencimiento = new DateTime(2024, 5, 10),
                Peso = 1,
                Descripcion = "Ya estan maduros",
                Estado_ = Ingrediente.Estado.Secundario
            });
            Ingredientes.Add(new Ingrediente
            {
                Nombre = "Platano",
                FechaVencimiento = new DateTime(2024, 6, 11),
                Peso = 1.1,
                Descripcion = "Estan verdes",
                Estado_ = Ingrediente.Estado.Secundario
            });
        }

        //Metodos para manejo de ingredientes
        public void AgregarIng()
        {
            Ingrediente IngAgregar= new Ingrediente();

            Console.WriteLine("Nombre del Ingrediente: ");
            IngAgregar.Nombre = Console.ReadLine() ?? "Sin Nombre";

            Console.WriteLine("Fecha vencimiento en d-M-yyyy porfa");
            string fechaStng= Console.ReadLine() ?? "31-12-2024";

            Console.WriteLine("Peso del Ingrediente (kg): ");
            IngAgregar.Peso = double.Parse(Console.ReadLine());

            Console.WriteLine("Valor Estado: Principal, Secundario o Decorativo");
            string estadoStng = Console.ReadLine() ?? "Secundario";

            Console.WriteLine("Descripcion adicional:");
            IngAgregar.Descripcion = Console.ReadLine() ?? "";

            try
            {
                DateTime fechaAgregar = DateTime.Parse(fechaStng);
                IngAgregar.FechaVencimiento=fechaAgregar;

                Ingrediente.Estado es = (Ingrediente.Estado)Enum.Parse(typeof(Ingrediente.Estado), estadoStng);
                IngAgregar.Estado_ = es;

            }
            catch (FormatException)
            {
                Console.WriteLine("Formato Incorrectisimo");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Valor de etiqueta no valido");
            }
            Ingredientes.Add(IngAgregar);
            //Tarea Agregada
            
        }

        public void EliminarIng()
        {
            Console.WriteLine("Nombre ingrediente a quitar");
            string nombreEli = Console.ReadLine() ?? "";

            Ingrediente ingEliminar= Ingredientes.FirstOrDefault(ing => ing.Nombre.Equals(nombreEli));

            if (ingEliminar != null)
            {
                Ingredientes.Remove(ingEliminar);
            }
            else
            {
                Console.WriteLine("No hay un ingrediente a eliminar");
            }
            Console.WriteLine();
        }

        public void EliminarIngChef(Ingrediente ingEliminar)
        {
            
           Ingrediente ingFin= Ingredientes.FirstOrDefault(ing => ing.Nombre.Equals(ingEliminar.Nombre));
            
          Ingredientes.Remove(ingFin);
            
           
            
        }

        public void ContarIng()
        {
            //contar por nombre, por fecha, por estado o en general
            Console.WriteLine("(D) Quieres contar por:\n 1. Nombre\n2. Fecha\n3. Estado\n4. En general");
            try { 
            int opcion=int.Parse(Console.ReadLine());
            
                switch (opcion)
                {
                    case 1:
                        ContarPor<string>("nombre", ing => ing.Nombre);
                        break;
                    case 2:
                        ContarPor<DateTime>("fecha", ing => ing.FechaVencimiento);
                        break;
                    case 3:
                        Console.WriteLine("Estado a contar: Principal, Secundario, Decorativo");
                        string estadoStng = Console.ReadLine() ?? "Secundario";
                        Ingrediente.Estado es = (Ingrediente.Estado)Enum.Parse(typeof(Ingrediente.Estado), estadoStng);
                        int ct = Ingredientes.Count(ing => ing.Estado_.Equals(es));
                        Console.WriteLine($"Cantidad de ingredientes con el Estado '{estadoStng}' : {ct}");
                        break;
                    case 4:
                        Contar_enGeneral();
                        break;
                    default:
                        Console.WriteLine("No selecciono algo valido");
                        break;

                }
            }
            catch (FormatException ex) { Console.Error.WriteLine($"Error: {ex.Message}"); }


        }

        private void ContarPor<T>(string tipo, Func<Ingrediente, T> selecionaAtributoCont)
        {
            try
            {
                Console.WriteLine($"Ingresa el {tipo} a contar:");
                T valor = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
                int cantidad = Ingredientes.Count(ing => EqualityComparer<T>.Default.Equals(selecionaAtributoCont(ing), valor));
                Console.WriteLine($"Cantidad de ingredientes con el {tipo} '{valor}': {cantidad}");
            }
            catch (Exception ex) { Console.WriteLine("Error en contar por " + ex); }
        }
        private void Contar_enGeneral()
        {
            int cantidad = 0;
            cantidad = Ingredientes.Count();
            Console.WriteLine($"Cantidad total Ingredientes {cantidad}");
        }

        public void FiltrarIng()
        {
            Console.WriteLine("Caracteristica a Filtar (Escriba): \nNombre\nFecha\nEstado");
            string atributo= Console.ReadLine().ToLower() ?? "nombre";
            List<Ingrediente> ingredientesFiltrados = Filtrar_porAtributo(atributo);
            if (ingredientesFiltrados != null)
            {
                foreach (var ing in ingredientesFiltrados)
                {
                    Console.WriteLine(ing.Imprimirse());
                }
            }
            
        }
        private List<Ingrediente> Filtrar_porAtributo(string atributoF)
        {
            switch(atributoF)
            {
                case "nombre":
                    Console.WriteLine($"{atributoF} a filtar:");
                    string nomFil = Console.ReadLine() ?? "";
                    return Ingredientes.Where(ing => ing.Nombre.Equals(nomFil)).ToList();
                    
                case "fecha":
                    Console.WriteLine($"{atributoF} a filtar (d-m-yyyy):");
                    string fechaStng = Console.ReadLine() ?? "1-01-2024";
                    DateTime fechaFil= DateTime.Parse(fechaStng);
                    return Ingredientes.Where(ing => ing.FechaVencimiento.Equals(fechaFil)).ToList();
                    
                case "estado":
                    Console.WriteLine($"{atributoF} a filtar:");
                    string estFil = Console.ReadLine() ?? "Secundario";
                    Ingrediente.Estado es= (Ingrediente.Estado)Enum.Parse(typeof(Ingrediente.Estado),estFil);
                    return Ingredientes.Where(ing => ing.Estado_.Equals(es)).ToList();
                    
                default:
                    Console.WriteLine("Opción de filtrado no valida");
                    return new List<Ingrediente>();
            }
        }

        public void VerIng()
        {
            foreach(var ingrediente in Ingredientes)
            {
                Console.WriteLine(ingrediente.Imprimirse());
            }
        }

        public void BuscarIng()
        {
            Console.WriteLine("Nombre ingrediente a buscar: ");
            string nomBuscar = Console.ReadLine() ?? "";
            Ingrediente ingBuscado= Ingredientes.FirstOrDefault(ing => ing.Nombre.Equals(nomBuscar));

            if(ingBuscado !=null)
            {
                Console.WriteLine(ingBuscado.Imprimirse());
            }
            else
            {
                Console.WriteLine($"No se encontro el ingrediente con nombre {nomBuscar}");
            }
        }
        public bool BuscarIngChef(Ingrediente ingBuscar)
        {
            bool esta = Ingredientes.Any(ing => ing.Nombre.Equals(ingBuscar.Nombre));
            return esta;
        }

        public void InfoPesosIng()
        {
            double valorP = Ingredientes.Average(ing => ing.Peso);
            double valorM= Ingredientes.Max(ing => ing.Peso);
            double valorm= Ingredientes.Min(ing => ing.Peso);
            Console.WriteLine($"El peso promedio de los ingredientes es de {Math.Round(valorP,3)} \n" +
                $"El peso maximo es de {valorM} y el minimo es de {valorm}");
        }

        public void OrdenarIng()
        {
            Console.WriteLine("Caracteristica a Ordenar (Escriba): \nNombre\nFecha\nPeso");
            string atributo = Console.ReadLine().ToLower() ?? "nombre";
            List<Ingrediente> ingOrdenados = Ordenar_porAtributo(atributo);
            if(ingOrdenados != null)
            foreach(var ingrediente in  ingOrdenados)
            {
                Console.WriteLine(ingrediente.Imprimirse());
            }
        }
        private List<Ingrediente> Ordenar_porAtributo(string atributoO)
        {
            switch (atributoO)
            {
                case "nombre":
                    return Ingredientes.OrderBy(ing => ing.Nombre).ToList();

                case "fecha":
                    return Ingredientes.OrderByDescending(ing => ing.FechaVencimiento).ToList();

                case "peso":
                    return Ingredientes.OrderBy(ing => ing.Peso).ToList();

                default:
                    Console.WriteLine("Opción de ordenado no valida");
                    return new List<Ingrediente>();
            }
        }

        public void ExportarDatosInventario()
        {
            try
            {
                Console.WriteLine("Ruta de archivo de Salida");
                string rutaStng = Console.ReadLine();
                StreamWriter escritor = new StreamWriter(rutaStng);
                string texto = ""; Ingrediente ing;

                for(int i=0;i<Ingredientes.Count;i++)
                {
                    ing = Ingredientes[i];
                    texto += ing.Imprimirse();
                }
                escritor.WriteLine(texto);
                escritor.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error "+ex);
            }
        }


    }
}
