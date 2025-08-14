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

    public IBlockingSelector selector { get; }

    public IBlockingPrompter<Orden> ordenPt { get; }
    public IBlockingPrompter<IngredienteEnStock> ingredientePt { get; }
    public IBlockingPrompter<int> intPt { get; }

    public IBlockingDisplayer<string> stringDp { get; }
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
        IBlockingSelector selector,
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

        this.selector = selector;
        this.productoDp = productoDp;
        this.ingredienteDp = ingredienteDp;
        this.ordenDp = ordenDp;
        this.stringDp = stringDp;

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
    public static void RestoreData
    (
        string dbServer,
        string dbName,
        string dbUser,
        string dbPassword
    ) => new EFPersistenceLayer(new Dictionary<string,string>{
            {"Host", dbServer},
            {"Database", dbName},
            {"Username", dbUser},
            {"Password", dbPassword},
        }).initData(true);
}