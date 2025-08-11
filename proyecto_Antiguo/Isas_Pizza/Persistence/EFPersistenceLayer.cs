using Isas_Pizza;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Isas_Pizza.Persistence
{
    /// <sumary>
    /// Representación de un Ingrediente en una base de datos soportada por EF
    /// </sumary>
    public record class EFIngrediente
    {
        /// <summary>Nombre del ingrediente.</summary>
        [Key]
        public string Nombre {get; set;}
        /// <summary>Descripción del tipo de ingrediente.</summary>
        public string Descripcion {get; set;}
        /// <summary>Unidades con las que se mide el ingrediente.</summary>
        public Unidad Unidad {get; set;}

        /// <summary>Convertir a Ingrediente</summary>
        public Ingrediente Export()
            => new Ingrediente {
                nombre = this.Nombre,
                descripcion = this.Descripcion,
                unidad = this.Unidad
            };

        public EFIngrediente(string Nombre, string Descripcion, Unidad Unidad)
        {
            this.Nombre = Nombre;
            this.Descripcion = Descripcion;
            this.Unidad = Unidad;
        }
        /// <summary>Construir basados en Ingrediente</summary>
        public EFIngrediente(Ingrediente ingrediente)
            : this(ingrediente.nombre,
                   ingrediente.descripcion,
                   ingrediente.unidad) {}
    }

    /// <sumary>
    /// Representación de un IngredienteEnStock en una base de datos soportada por EF
    /// </sumary>
    public record class EFIngredienteEnStock
    {
        /// <summary>Id del ingrediente.</summary>
        [Key]
        [ForeignKey(nameof(Ingrediente))]
        public string IngredienteNombre {get; set;}
        public EFIngrediente Ingrediente {get; set;}
        /// <summary>Cantidad asociada al ingrediente.</summary>
        public double Cantidad {get; set;}
        /// <summary>Descripción del tipo de ingrediente.</summary>
        public DateTime FechaVencimiento {get; set;}

        /// <summary>Convertir a IngredienteEnStock</summary>
        public IngredienteEnStock Export()
            => new IngredienteEnStock {
                ingrediente = this.Ingrediente != null ? this.Ingrediente.Export() : null,
                cantidad = this.Cantidad,
                fechaVencimiento = this.FechaVencimiento
            };
        
        public EFIngredienteEnStock(){}

        /// <summary>Construir basados en IngredienteEnStock</summary>
        public EFIngredienteEnStock(IngredienteEnStock ingredienteEnStock)
        {
            this.IngredienteNombre = ingredienteEnStock.ingrediente.nombre;
            this.Ingrediente = new EFIngrediente(ingredienteEnStock.ingrediente);
            this.Cantidad = ingredienteEnStock.cantidad;
            this.FechaVencimiento = ingredienteEnStock.fechaVencimiento;
        }

        public EFIngredienteEnStock(IngredienteEnStock ingredienteEnStock,
                                    EFContext ctx)
            : this(ingredienteEnStock)
        {
            this.Ingrediente = ctx.Ingredientes.Single(i => i.Nombre == this.IngredienteNombre);
        }
    }

    /// <summary>
    /// Contexto para una base de datos SQLite
    /// </summary>
    public class EFContext : DbContext
    {
        public DbSet<EFIngrediente> Ingredientes {get; set;}
        public DbSet<EFIngredienteEnStock> IngredientesEnStock {get; set;}
        // public DbSet<EFIngredienteEnStock> IngredientesEnStock {get; set;}

        /// <summary>
        /// Configurar la conexión a la base de datos.
        /// </summary>
        /// \todo parametrizar el nombre de la conexión.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource = /home/fer/.pizzeria.db; Cache=Shared");
    }

    public class EFPersistenceLayer :
        IROPersistenceLayer<Ingrediente>,
        IPersistenceLayer<IngredienteEnStock>
    {
        /// <summary>
        /// Contexto de conexión a la base de datos
        /// </summary>
        private EFContext _context;

        public EFPersistenceLayer()
        {
            this._context = new EFContext();
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
            this._context.SaveChanges();
            this._context.Ingredientes.AddRange(InitData.ingredientes);
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
    }
}
