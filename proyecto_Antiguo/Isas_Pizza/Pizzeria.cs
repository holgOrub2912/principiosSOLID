using System;
using System.CommandLine;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Isas_Pizza;
using Isas_Pizza.IO;
using Isas_Pizza.Persistence;

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
        string authFile,
        string dbServer,
        string dbName,
        string dbUser,
        string dbPassword,
        IBlockingDisplayer<Producto> productoDp,
        IBlockingDisplayer<IngredienteEnStock> ingredienteDp,
        IBlockingDisplayer<Orden> ordenDp,
        IBlockingDisplayer<string> stringDp,
        Func<IEnumerable<Ingrediente>, IBlockingPrompter<IngredienteEnStock>> ingredientePtGen,
        Func<IEnumerable<Producto>, IBlockingPrompter<Orden>> ordenPtGen
    )
    {
        this.auth = new GenericAuthenticator(new AuthPersistenceLayer(authFile), stringDp);

        EFPersistenceLayer dataStorage = new EFPersistenceLayer(new Dictionary<string,string>{
            {"Host", dbServer},
            {"Database", dbName},
            {"Username", dbUser},
            {"Password", dbPassword},
        });
        this.inventario = dataStorage;
        this.ingredientes = dataStorage;
        this.menu = dataStorage;
        this.ordenes = dataStorage;

        this.productoDp = productoDp;
        this.ingredienteDp = ingredienteDp;
        this.ordenDp = ordenDp;

        this.ingredientePt = ingredientePtGen(this.ingredientes.View(null));
        this.ordenPt = ordenPtGen(this.menu.View(null));
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
        Option<string> dbserver = new("--dbserver", "-s")
        {
            Description = "Host de base de datos al cual conectarse.",
            DefaultValueFactory = parseResult => DefaultParameters.dbServer,
        };
        Option<string> dbname = new("--dbname", "-d")
        {
            Description = "Nombre de la base de datos a la que conectarse.",
            DefaultValueFactory = parseResult => DefaultParameters.dbName,
        };
        Option<string> dbuser = new("--dbuser", "-u")
        {
            Description = "Nombre de usuario para conexión a la base de datos.",
            DefaultValueFactory = parseResult => DefaultParameters.dbUser,
        };
        Option<string> dbpassword = new("--dbpassword", "-p")
        {
            Description = "Contraseña para conexión a la base de datos.",
            DefaultValueFactory = parseResult => DefaultParameters.dbPassword,
        };
        Option<string> authfile = new("--authfile", "-p")
        {
            Description = "Archivo de credenciales",
            DefaultValueFactory = parseResult => DefaultParameters.authfile,
        };
        RootCommand rootCommand = new("Isa's Pizza")
        {
            dbserver,
            dbname,
            dbuser,
            dbpassword,
            authfile,
        };
        ParseResult cmdArgs = rootCommand.Parse(args);

        IBlockingPrompter<LoginCredentials?> loginPrompter = new CredentialPrompter();
        PrimitiveIO defaultPrimitiveIO = new PrimitiveIO();

        IBlockingSelector menuSelector = new MenuGenericoIO();

        try {
            Pizzeria pizzeria = new Pizzeria(
                cmdArgs.GetValue(authfile),
                cmdArgs.GetValue(dbserver),
                cmdArgs.GetValue(dbname),
                cmdArgs.GetValue(dbuser),
                cmdArgs.GetValue(dbpassword),
                new ProductoIO(),
                new IngredienteIO(menuSelector, []),
                new OrdenIO(menuSelector, []),
                new PrimitiveIO(),
                ings => new IngredienteIO(menuSelector, ings),
                prods => new OrdenIO(menuSelector, prods)
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