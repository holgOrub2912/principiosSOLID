using Isas_Pizza;

class Pizzeria
{
    public static void Main(string[] args)
    {
        Inventario inventario = new Inventario();
        Administrador admin= new Administrador(inventario);
        Chef juaquin=new Chef();

        //suscribirme eventos
        admin.Orden_Lista += juaquin.EventHandler_OrdenLista;
        admin.Sesion_Iniciada += EventHandler_SesionIniciada;

        admin.EscogerMenu();
    }
    public static void EventHandler_SesionIniciada(object sender, IDArgs id_) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Inicio de sesión exitoso, desea ver su ID? (Y/N)");
        string confir= Console.ReadLine().ToUpper() ?? "N";
        if (confir.Equals("Y"))
        {
            Console.WriteLine(id_.ID);
        }
        Console.ResetColor();
    }
}
