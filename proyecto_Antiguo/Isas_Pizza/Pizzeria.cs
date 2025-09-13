using System;
using System.CommandLine;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Isas_Pizza;
using Isas_Pizza.IO;
using Isas_Pizza.Models;
using Isas_Pizza.Persistence;
using Isas_Pizza.UserMenus;

public class Pizzeria
{
    public IROPersistenceLayer<Ingrediente> ingredientes => EFPersistenceLayer.Instance;
    public IPersistenceLayer<IngredienteEnStock> inventario => EFPersistenceLayer.Instance;
    public IPersistenceLayer<Producto> menu;
    public IPersistenceLayer<Orden> ordenes => EFPersistenceLayer.Instance;

    public IUserAgent? usuarioActivo { get; private set; } = null;

    IAuthenticator auth { get; }
    public IIOFacade io { get; }

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
        Func<Pizzeria, IIOFacade> ioFacadeGen
    )
    {
        EFPersistenceLayer.Init(new Dictionary<string,string>{
            {"Host", dbServer},
            {"Database", dbName},
            {"Username", dbUser},
            {"Password", dbPassword},
        });
        io = ioFacadeGen(this);
        menu = new MenuFilter(
            EFPersistenceLayer.Instance,
            EFPersistenceLayer.Instance
        );

        this.auth = new GenericAuthenticator(new AuthPersistenceLayer(authFile), io);
    }
    
    public void LogIn(IBlockingPrompter<LoginCredentials?> prompter)
    {
        this.usuarioActivo = auth.Authenticate(prompter);
    }
    public IEnumerable<(string label, Action<Pizzeria>)> Menu()
        => usuarioActivo?.GetMenu()
                        .Menu()
                        .Append(("Salir", LogOut)) ?? [];

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
    )
    {
        EFPersistenceLayer.Init(new Dictionary<string, string>{
            {"Host", dbServer},
            {"Database", dbName},
            {"Username", dbUser},
            {"Password", dbPassword},
        });
        EFPersistenceLayer.Instance.initData(true);
    }
}