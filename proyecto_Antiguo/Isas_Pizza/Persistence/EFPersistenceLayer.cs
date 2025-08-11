using Isas_Pizza;
using Isas_Pizza.Persistence.EFModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.Contracts;

namespace Isas_Pizza.Persistence
{
    /// <summary>
    /// Contexto para una base de datos SQLite
    /// </summary>
    public class EFContext : DbContext
    {
        public DbSet<EFIngrediente> Ingredientes {get; set;}
        public DbSet<EFIngredienteEnStock> IngredientesEnStock {get; set;}
        public DbSet<EFIngredienteCantidad> IngredientesCantidad {get; set;}
        public DbSet<EFProducto> Productos {get; set;}
        public DbSet<EFProductoOrden> ProductosOrdenes {get; set;}
        public DbSet<EFOrden> Ordenes {get; set;}
        public string _dbpath;

        public EFContext(string dbpath)
        {
            this._dbpath = dbpath;
        }
        // public DbSet<EFIngredienteEnStock> IngredientesEnStock {get; set;}

        /// <summary>
        /// Configurar la conexión a la base de datos.
        /// </summary>
        /// \todo parametrizar el nombre de la conexión.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"DataSource = {this._dbpath}; Cache=Shared");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EFProductoOrden>()
                .HasKey(po => new { po.NombreProducto, po.NumeroOrden });
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
            return new EFContext(args[0]);
        }
    }

    public class EFPersistenceLayer :
        IROPersistenceLayer<Ingrediente>,
        IPersistenceLayer<IngredienteEnStock>,
        IROPersistenceLayer<Producto>,
        IPersistenceLayer<Orden>
    {
        /// <summary>
        /// Contexto de conexión a la base de datos
        /// </summary>
        private EFContext _context;

        public EFPersistenceLayer(string dbpath)
        {
            this._context = new EFContext(dbpath);
        }

        /// <summary>
        /// Inicializar datos de ejemplo
        /// </summary>
        public void _initData()
        {
            ICollection<EFIngrediente> ingredientes = this._context.Ingredientes.ToList();
            if (ingredientes.Count() > 0)
            {
                this._context.Ingredientes.RemoveRange(ingredientes);
            }
            ICollection<EFProducto> productos = this._context.Productos.ToList();
            if (productos.Count() > 0)
            {
                this._context.Productos.RemoveRange(productos);
            }
            ICollection<EFOrden> ordenes = this._context.Ordenes.ToList();
            if (ordenes.Count() > 0)
            {
                this._context.Ordenes.RemoveRange(ordenes);
            }
            this._context.SaveChanges();
            this._context.Ingredientes.AddRange(InitData.ingredientes);
            this._context.Productos.AddRange(InitData.productos);
            this._context.SaveChanges();
        }

        public IEnumerable<Ingrediente> View(Ingrediente? _)
            => (IEnumerable<Ingrediente>) this._context.Ingredientes
                .ToList()
                .Select(ing => ing.Export());

        public IEnumerable<IngredienteEnStock> View(IngredienteEnStock? _)
            => (IEnumerable<IngredienteEnStock>) this._context.IngredientesEnStock
                .ToList()
                .Select(ing => ing.Export());

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
                .ToList()
                .Select(p => p.Export());

        public IEnumerable<Orden> View(Orden? _)
            => this._context.Ordenes.Select(o => o.Export());
        
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