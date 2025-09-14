using Isas_Pizza;
using Isas_Pizza.Models;
using Isas_Pizza.Persistence.EFModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using Microsoft.Extensions.Options;
using System.CommandLine;
using System.Globalization;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace Isas_Pizza.Persistence
{
    public class EFContext : DbContext
    {
        public DbSet<EFIngrediente> Ingredientes {get; set;}
        public DbSet<EFIngredienteEnStock> IngredientesEnStock {get; set;}
        public DbSet<EFIngredienteCantidad> IngredientesCantidad {get; set;}
        public DbSet<EFProducto> Productos {get; set;}
        public DbSet<EFProductoOrden> ProductosOrdenes {get; set;}
        public DbSet<EFOrden> Ordenes {get; set;}
        private readonly string _connectionString;

        /// <summary>
        /// Recibir los parámetros de conexión.
        /// </summary>
        /// <param name="connectionOptions">Parámetros ce conexión a la base de datos.</param>
        public EFContext(IDictionary<string, string> connectionOptions)
        {
            this._connectionString  = string.Join(";",
                connectionOptions
                    .Select(item => $"{item.Key}={item.Value}")
            ) + ";Pooling=false";
        }
        // public DbSet<EFIngredienteEnStock> IngredientesEnStock {get; set;}

        /// <summary>
        /// Configurar la conexión a la base de datos.
        /// </summary>
        /// \todo parametrizar el nombre de la conexión.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql(this._connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EFProductoOrden>()
                .HasKey(po => new { po.NombreProducto, po.NumeroOrden });
            modelBuilder.Entity<EFOrden>()
                .HasMany(o => o.ProductosOrdenados)
                .WithOne(po => po.Orden)
                .HasForeignKey(po => po.NumeroOrden);
            modelBuilder.Entity<EFProductoOrden>()
                .HasOne(po => po.Producto)
                .WithMany()
                .HasForeignKey(po => po.NombreProducto);
        }

    }

    /// <summary>
    /// Design-time dbcontext factory to manage migrations and database
    /// updates from dotnet ef CLI
    /// </summary>
    public class EFContextFactory : IDesignTimeDbContextFactory<EFContext>
    {
        private IDictionary<string, string> _dbOptions;
        public EFContextFactory(){}
        public EFContextFactory(IDictionary<string, string> dbOptions)
        {
            this._dbOptions = dbOptions;
        }
        public EFContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<EFContext> optionsBuilder = new();
            Option<string> server = new("--server")
            {
                DefaultValueFactory = results => DefaultParameters.dbServer
            };
            Option<string> database = new("--database")
            {
                DefaultValueFactory = results => DefaultParameters.dbName
            };
            Option<string> user = new("--user")
            {
                DefaultValueFactory = results => DefaultParameters.dbUser
            };
            Option<string> password = new("--password")
            {
                DefaultValueFactory = results => DefaultParameters.dbPassword
            };

            RootCommand rootCommand = new("Design EFPersistenceLayer module")
            {
                user,
                password,
                server,
                database
            };
            ParseResult parseResult = rootCommand.Parse(args);

            _dbOptions = new Dictionary<string, string>
            {
                { "Host", parseResult.GetValue(server) },
                { "Username", parseResult.GetValue(user) },
                { "Database", parseResult.GetValue(database) },
                { "Password", parseResult.GetValue(password) },
            };
            return new EFContext(_dbOptions);
        }
        public EFContext CreateDbContext()
            => new EFContext(_dbOptions);
    }

    public class EFPersistenceLayer :
        IROPersistenceLayer<Ingrediente>,
        IPersistenceLayer<IngredienteEnStock>,
        IPersistenceLayer<Producto>,
        IPersistenceLayer<Orden>
    {
        /// <summary>
        /// Contexto de conexión a la base de datos
        /// </summary>
        private EFContextFactory _contextFactory;
        private EFContext? _temporaryContext;

        public static EFPersistenceLayer Instance { get; private set; }

        public static void Init(IDictionary<string,string> dbOptions)
        {
            if (Instance is null)
                Instance = new EFPersistenceLayer(dbOptions);
            else
                throw new InvalidOperationException("Tratando de inicializar EFPersistenceLayer más de una vez");
        }


        private EFPersistenceLayer(IDictionary<string,string> dbOptions)
        {
            this._contextFactory = new EFContextFactory(dbOptions);
            EFContext context = this._contextFactory.CreateDbContext();
            // Verificar la integridad de la base de datos
            try {
                context.Ingredientes.FirstOrDefault();
                context.IngredientesCantidad.FirstOrDefault();
                context.IngredientesEnStock.FirstOrDefault();
                context.Productos.FirstOrDefault();
                context.ProductosOrdenes.FirstOrDefault();
                context.Ordenes.FirstOrDefault();
            } catch {
                throw new PersistenceException($"Al leer la base de datos seleccionada.");
            }
        }

        /// <summary>
        /// Inicializar datos de ejemplo
        /// </summary>
        public void initData(bool fromScratch)
        {
            using EFContext context = this._contextFactory.CreateDbContext();
            ICollection<EFIngrediente> ingredientes = context.Ingredientes.ToList();
            if (fromScratch || ingredientes.Count > 0)
            {
                context.Ingredientes.RemoveRange(ingredientes);
            }
            ICollection<EFProducto> productos = context.Productos.ToList();
            if (fromScratch || productos.Count > 0)
            {
                context.Productos.RemoveRange(productos);
            }
            ICollection<EFOrden> ordenes = context.Ordenes.ToList();
            if (fromScratch || ordenes.Count > 0)
            {
                context.Ordenes.RemoveRange(ordenes);
            }
            ICollection<EFIngredienteEnStock> stock = context.IngredientesEnStock.ToList();
            if (fromScratch || ordenes.Count > 0)
            {
                context.IngredientesEnStock.RemoveRange(stock);
            }
            context.SaveChanges();
            context.Ingredientes.AddRange(InitData.ingredientes);
            context.Productos.AddRange(InitData.productos);
            context.IngredientesEnStock.AddRange(InitData.existencias);
            context.SaveChanges();
        }

        private EFContext GetContext()
            => this._temporaryContext ?? this._contextFactory.CreateDbContext();
        public void BeginTransaction()
        {
            this._temporaryContext = this._contextFactory.CreateDbContext();
            this._temporaryContext.Database.BeginTransaction();
        }
        public void Clear()
        {
            if (this._temporaryContext is null)
                return;
            this._temporaryContext.Database.RollbackTransaction();
            this._temporaryContext.ChangeTracker.Clear();
            this._temporaryContext = null;
        }

        public IEnumerable<Ingrediente> View(Ingrediente? _)
        {
            EFContext context = GetContext();
            return (IEnumerable<Ingrediente>) context.Ingredientes
                .AsNoTracking()
                .ToList()
                .Select(ing => ing.Export());

        }
        public IEnumerable<IngredienteEnStock> View(IngredienteEnStock? _)
        {
            EFContext context = GetContext();
            return (IEnumerable<IngredienteEnStock>) context.IngredientesEnStock
                .AsNoTracking()
                .Include(ies => ies.Ingrediente) 
                .ToList()
                .Select(ing => ing.Export());
        }

        public void Save(IEnumerable<IngredienteEnStock> ingredientesES)
        {
            EFContext context = GetContext();
            IEnumerable<EFIngredienteEnStock> ingsES
                = ingredientesES.Select(
                    ing => new EFIngredienteEnStock(ing, context)
                );
            
            context.AddRange(ingsES);
            // Save on a shallow fashion
            // Do not create the underlying Ingrediente
            foreach (EFIngredienteEnStock ingES in ingsES)
                context.Entry(ingES.Ingrediente).State = EntityState.Unchanged;
            context.SaveChanges();
        }

        private EFIngredienteEnStock RetrieveIES(IngredienteEnStock source, EFContext context)
            => context.IngredientesEnStock.Single(ies =>
                ies.IngredienteNombre== source.ingrediente.nombre);

        public void Update(IngredienteEnStock source, IngredienteEnStock target)
        {
            EFContext context = GetContext();
            EFIngredienteEnStock ingES = this.RetrieveIES(source, context);
            ingES.Cantidad = target.cantidad;
            ingES.FechaVencimiento = target.fechaVencimiento;
            context.SaveChanges();
        }

        public void Delete(IngredienteEnStock target)
        {
            EFContext context = GetContext();
            EFIngredienteEnStock ingES = this.RetrieveIES(target, context);
            context.IngredientesEnStock.Remove(ingES);
            context.SaveChanges();
        }

        public IEnumerable<Producto> View(Producto? _)
        {
            EFContext context = GetContext();
            
            return (IEnumerable<Producto>) context.Productos
                .Include(p => p.IngredientesRequeridos)
                .ThenInclude(ingreq => ingreq.Ingrediente)
                .ToList()
                .Select(p => p.Export());
        }

        public void Save(IEnumerable<Producto> productos)
        {
            EFContext context = GetContext();
            IEnumerable<EFProducto> efProductos = productos
                .Select(p => new EFProducto(p, context));

            foreach (EFIngrediente ing in
                efProductos.SelectMany(p => p.IngredientesRequeridos,
                                      (p, ingreq) => ingreq.Ingrediente)
                    )
                context.Entry(ing).State = EntityState.Unchanged;

            context.AddRange(efProductos);
            context.SaveChanges();
        }

        public void Delete(Producto producto)
        {
            EFContext context = GetContext();
            EFProducto efProducto = context.Productos
                .Single(p => p.Nombre == producto.nombre);
            context.Productos.Remove(efProducto);
            context.SaveChanges();
        }

        /// <summary>
        /// Actualizar producto
        /// </summary>
        /// <param name="source">Producto original</param>
        /// <param name="target">Producto actualizado</param>
        /// \todo Ver si nombre del producto se deja cambiar o no
        public void Update(Producto source, Producto target)
        {
            EFContext context = GetContext();
            EFProducto efProducto = context.Productos
                .Single(p => p.Nombre == source.nombre);

            // efProducto.Nombre = target.nombre;
            efProducto.Precio = target.precio;
            efProducto.IngredientesRequeridos = target.ingredientesRequeridos
                .Select(ing => new EFIngredienteCantidad(ing))
                .ToArray();
            context.SaveChanges();
        }

        public IEnumerable<Orden> View(Orden? _)
        {
            return GetContext().Ordenes
                .AsNoTracking()
                .Include(o => o.ProductosOrdenados)
                .ThenInclude(po => po.Producto)
                .ThenInclude(p => p.IngredientesRequeridos)
                .ThenInclude(ic => ic.Ingrediente)
                .Select(o => o.Export());
        }
        
        public void Save(IEnumerable<Orden> ordenes)
        {
            EFContext context = GetContext();
            IEnumerable<EFOrden> efOrdenes = ordenes
                .Select(o => new EFOrden(o, context));

            context.AddRange(efOrdenes);
            context.SaveChanges();
        }

        private EFOrden RetrieveEFOrden(Orden source, EFContext context)
            => context.Ordenes
                .Single(o => o.NumeroOrden == source.numeroOrden);
        public void Update(Orden source, Orden target)
        {
            EFContext context = GetContext();
            EFOrden efOrden = RetrieveEFOrden(source, context);
            efOrden.Estado = target.estado;
            context.SaveChanges();
        }
        public void Delete(Orden orden)
        {
            EFContext context = GetContext();
            EFOrden efOrden = RetrieveEFOrden(orden, context);
            context.Ordenes.Remove(efOrden);
            context.SaveChanges();
        }

    }
}