using System.CommandLine;
using Isas_Pizza.Persistence;
using Isas_Pizza;

namespace Isas_Pizza.IO
{
    public class ConsoleClient
    {
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
            Option<bool> resetDatabase = new("--resetdb", "-r")
            {
                Description = "Restaurar valores en la base de datos.",
                DefaultValueFactory = parseResult => false
            };
            RootCommand rootCommand = new("Isa's Pizza")
            {
                dbserver,
                dbname,
                dbuser,
                dbpassword,
                authfile,
                resetDatabase,
            };
            ParseResult cmdArgs = rootCommand.Parse(args);

            IBlockingSelector menuSelector = new MenuGenericoIO();
    
            IBlockingPrompter<LoginCredentials?> loginPrompter = new CredentialPrompter();
            PrimitiveIO defaultPrimitiveIO = new PrimitiveIO();
    
            try {
                if (cmdArgs.GetValue(resetDatabase)){
                    Console.WriteLine("¿Estás seguro que quieres restaurar la base de datos?");
                    if (
                        menuSelector.SelectOne<bool>([("Sí, estoy seguro", true), ("No, no restaurar", false)]) 
                    )
                        Pizzeria.RestoreData(
                            cmdArgs.GetValue(dbserver),
                            cmdArgs.GetValue(dbname),
                            cmdArgs.GetValue(dbuser),
                            cmdArgs.GetValue(dbpassword)
                        );
                    return;
                }

                Pizzeria pizzeria = new Pizzeria(
                    cmdArgs.GetValue(authfile),
                    cmdArgs.GetValue(dbserver),
                    cmdArgs.GetValue(dbname),
                    cmdArgs.GetValue(dbuser),
                    cmdArgs.GetValue(dbpassword),
                    menuSelector,
                    new ProductoIO(),
                    new IngredienteIO(menuSelector, []),
                    new OrdenIO(menuSelector, () => []),
                    defaultPrimitiveIO,
                    ings => new IngredienteIO(menuSelector, ings),
                    prods => new OrdenIO(menuSelector, prods),
                    defaultPrimitiveIO,
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
    }
}