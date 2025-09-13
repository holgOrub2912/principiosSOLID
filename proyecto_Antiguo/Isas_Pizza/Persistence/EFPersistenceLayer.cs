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

            Dictionary<string, string> dbOptions = new Dictionary<string, string>
            {
                { "Host", parseResult.GetValue(server) },
                { "Username", parseResult.GetValue(user) },
                { "Database", parseResult.GetValue(database) },
                { "Password", parseResult.GetValue(password) },
            };
            return new EFContext(dbOptions);
        }
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
        private EFContext _context;

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
            this._context = new EFContext(dbOptions);
            // Verificar la integridad de la base de datos
            try {
                this._context.Ingredientes.FirstOrDefault();
                this._context.IngredientesCantidad.FirstOrDefault();
                this._context.IngredientesEnStock.FirstOrDefault();
                this._context.Productos.FirstOrDefault();
                this._context.ProductosOrdenes.FirstOrDefault();
                this._context.Ordenes.FirstOrDefault();
            } catch {
                throw new PersistenceException($"Al leer la base de datos seleccionada.");
            }
        }

        /// <summary>
        /// Inicializar datos de ejemplo
        /// </summary>
        public void initData(bool fromScratch)
        {
            ICollection<EFIngrediente> ingredientes = this._context.Ingredientes.ToList();
            if (fromScratch || ingredientes.Count > 0)
            {
                this._context.Ingredientes.RemoveRange(ingredientes);
            }
            ICollection<EFProducto> productos = this._context.Productos.ToList();
            if (fromScratch || productos.Count > 0)
            {
                this._context.Productos.RemoveRange(productos);
            }
            ICollection<EFOrden> ordenes = this._context.Ordenes.ToList();
            if (fromScratch || ordenes.Count > 0)
            {
                this._context.Ordenes.RemoveRange(ordenes);
            }
            ICollection<EFIngredienteEnStock> stock = this._context.IngredientesEnStock.ToList();
            if (fromScratch || ordenes.Count > 0)
            {
                this._context.IngredientesEnStock.RemoveRange(stock);
            }
            this._context.SaveChanges();
            this._context.Ingredientes.AddRange(InitData.ingredientes);
            this._context.Productos.AddRange(InitData.productos);
            this._context.IngredientesEnStock.AddRange(InitData.existencias);
            this._context.SaveChanges();
        }

        public IEnumerable<Ingrediente> View(Ingrediente? _)
            => (IEnumerable<Ingrediente>) this._context.Ingredientes
                .AsNoTracking()
                .ToList()
                .Select(ing => ing.Export());

        public IEnumerable<IngredienteEnStock> View(IngredienteEnStock? _)
        {
            return (IEnumerable<IngredienteEnStock>)this._context.IngredientesEnStock
                .AsNoTracking()
                .Include(ies => ies.Ingrediente) 
                .ToList()
                .Select(ing => ing.Export());
        }

        public void Save(IEnumerable<IngredienteEnStock> ingredientesES)
        {
            IEnumerable<EFIngredienteEnStock> ingsES
                = ingredientesES.Select(
                    ing => new EFIngredienteEnStock(ing, this._context)
                );
            
            this._context.AddRange(ingsES);
            // Save on a shallow fashion
            // Do not create the underlying Ingrediente
            foreach (EFIngredienteEnStock ingES in ingsES)
                this._context.Entry(ingES.Ingrediente).State = EntityState.Unchanged;
            this._context.SaveChanges();
        }

        private EFIngredienteEnStock RetrieveIES(IngredienteEnStock source)
            => this._context.IngredientesEnStock.Single(ies =>
                ies.IngredienteNombre== source.ingrediente.nombre);

        public void Update(IngredienteEnStock source, IngredienteEnStock target)
        {
            EFIngredienteEnStock ingES = this.RetrieveIES(source);
            ingES.Cantidad = target.cantidad;
            ingES.FechaVencimiento = target.fechaVencimiento;
            this._context.SaveChanges();
        }

        public void Delete(IngredienteEnStock target)
        {
            EFIngredienteEnStock ingES = this.RetrieveIES(target);
            this._context.IngredientesEnStock.Remove(ingES);
            this._context.SaveChanges();
        }

        public IEnumerable<Producto> View(Producto? _)
            => (IEnumerable<Producto>) this._context.Productos
                .Include(p => p.IngredientesRequeridos)
                .ThenInclude(ingreq => ingreq.Ingrediente)
                .ToList()
                .Select(p => p.Export());

        public void Save(IEnumerable<Producto> productos)
        {
            IEnumerable<EFProducto> efProductos = productos
                .Select(p => new EFProducto(p, this._context));

            foreach (EFIngrediente ing in
                efProductos.SelectMany(p => p.IngredientesRequeridos,
                                      (p, ingreq) => ingreq.Ingrediente)
                    )
                this._context.Entry(ing).State = EntityState.Unchanged;

            this._context.AddRange(efProductos);
            this._context.SaveChanges();
        }

        public void Delete(Producto producto)
        {
            EFProducto efProducto = this._context.Productos
                .Single(p => p.Nombre == producto.nombre);
            this._context.Productos.Remove(efProducto);
            this._context.SaveChanges();
        }

        /// <summary>
        /// Actualizar producto
        /// </summary>
        /// <param name="source">Producto original</param>
        /// <param name="target">Producto actualizado</param>
        /// \todo Ver si nombre del producto se deja cambiar o no
        public void Update(Producto source, Producto target)
        {
            EFProducto efProducto = this._context.Productos
                .Single(p => p.Nombre == source.nombre);

            // efProducto.Nombre = target.nombre;
            efProducto.Precio = target.precio;
            efProducto.IngredientesRequeridos = target.ingredientesRequeridos
                .Select(ing => new EFIngredienteCantidad(ing))
                .ToArray();
            this._context.SaveChanges();
        }

        public IEnumerable<Orden> View(Orden? _)
        {
            return this._context.Ordenes
                .AsNoTracking()
                .Include(o => o.ProductosOrdenados)
                .ThenInclude(po => po.Producto)
                .ThenInclude(p => p.IngredientesRequeridos)
                .ThenInclude(ic => ic.Ingrediente)
                .Select(o => o.Export());
        }
        
        public void Save(IEnumerable<Orden> ordenes)
        {
            IEnumerable<EFOrden> efOrdenes = ordenes
                .Select(o => new EFOrden(o, this._context));

            this._context.AddRange(efOrdenes);
            this._context.SaveChanges();
        }

        private EFOrden RetrieveEFOrden(Orden source)
            => this._context.Ordenes
                .Single(o => o.NumeroOrden == source.numeroOrden);
        public void Update(Orden source, Orden target)
        {
            EFOrden efOrden = RetrieveEFOrden(source);
            efOrden.Estado = target.estado;
            this._context.SaveChanges();
        }
        public void Delete(Orden orden)
        {
            EFOrden efOrden = RetrieveEFOrden(orden);
            this._context.Ordenes.Remove(efOrden);
            this._context.SaveChanges();
        }

    }
}