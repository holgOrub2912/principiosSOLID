using System;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Isas_Pizza;
using Isas_Pizza.IO;
using Isas_Pizza.Persistence;
using SQLitePCL;

public class Pizzeria
{
    public IROPersistenceLayer<Ingrediente> ingredientes { get; }
    public IPersistenceLayer<IngredienteEnStock> inventario { get; }
    public IROPersistenceLayer<Producto> menu { get; }
    public IPersistenceLayer<Orden> ordenes { get; }

    public IUserAgent? usuarioActivo { get; private set; } = null;

    IAuthenticator auth { get; }

    public IBlockingPrompter<Orden> ordenPt { get; }
    public IBlockingPrompter<IngredienteEnStock> ingredientePt { get; }
    public IBlockingPrompter<int> intPt { get; }

    public IBlockingDisplayer<Orden> ordenDp { get; }
    public IBlockingDisplayer<IngredienteEnStock> ingredienteDp { get; }
    public IBlockingDisplayer<Producto> productoDp { get; }

    /// <summary>
    /// Inicializar capa de persistencia e interfaz.
    /// </summary>
    public Pizzeria
    (
        string dbPath,
        string authPath,
        IBlockingDisplayer<Producto> productoDp,
        IBlockingDisplayer<IngredienteEnStock> ingredienteDp,
        IBlockingDisplayer<string> stringDp
    )
    {
        this.productoDp = productoDp;
        this.auth = new GenericAuthenticator(new AuthPersistenceLayer(authPath), stringDp);
        EFPersistenceLayer dataStorage = new EFPersistenceLayer(dbPath);
        
        this.inventario = dataStorage;
        this.ingredientes = dataStorage;
        this.menu = dataStorage;
        this.ordenes = dataStorage;
    }

    public void LogIn(IBlockingPrompter<LoginCredentials?> prompter)
    {
        this.usuarioActivo = auth.Authenticate(prompter);
    }
    public IEnumerable<(string label, Action<Pizzeria>)> Menu()
        => (usuarioActivo.GetRole() switch
        {
            UserRole.CONSUMIDOR => (new ConsumidorMenu()).Menu(),
            UserRole.ADMINISTRADOR => (new AdministradorMenu()).Menu(),
            UserRole.CHEF => (new ChefMenu()).Menu(),
            _ => throw new NotImplementedException("El menú para este rol no ha sido implementado.")
        }).Append(("Salir", LogOut));

    public static void LogOut(Pizzeria pizzeria)
    {
        pizzeria.usuarioActivo = null;
    }

    public static void Main(string[] args)
    {
        if (args.Length < 2){
            Console.WriteLine("Especificar localización de base de datos y archivo de autenticación por favor.");
            return;
        }

        IBlockingPrompter<LoginCredentials?> loginPrompter = new CredentialPrompter();
        IBlockingDisplayer<IngredienteEnStock> ingredienteDp = new IngredienteIO(new IngredienteEnStock[1]);
        PrimitiveIO defaultPrimitiveIO = new PrimitiveIO();

        IBlockingSelector menuSelector = new MenuGenericoIO();

        try {
            Pizzeria pizzeria = new Pizzeria(
                args[0],
                args[1],
                new ProductoIO([new Producto{}]),
                ingredienteDp,
                defaultPrimitiveIO
            );
            pizzeria.LogIn(loginPrompter);
            while (pizzeria.usuarioActivo is not null)
                menuSelector.SelectOne(
                    pizzeria.Menu().ToList()
                )(pizzeria);
        } catch(PersistenceException e) {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    internal static void VerInventario(Pizzeria pizzeria)
    {
        throw new NotImplementedException();
    }
}