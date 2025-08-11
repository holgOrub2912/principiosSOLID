using Isas_Pizza;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.Contracts;

/// <summary>
/// Modelos necesarios para la capa de persistencia EFPersistenceLayer
/// </summary>
namespace Isas_Pizza.Persistence.EFModel
{
    /// <summary>
    /// Representación de un Ingrediente en una base de datos soportada
    /// por EF.
    /// </summary>
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

    /// <summary>
    /// Representación de un IngredienteEnStock en una base de datos
    /// soportada por EF
    /// </summary>
    public record class EFIngredienteEnStock
    {
        /// <summary>Nombre del ingrediente al cual se refiere (llave
        /// foránea).</summary>
        [Key]
        [ForeignKey(nameof(Ingrediente))]
        public string IngredienteNombre {get; set;}
        /// <summary>Referencia al ingrediente en cuestión.</summary>
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
    /// Representación de un IngredienteCantidad en una base de datos
    /// soportada por EF, asociada al producto del cual  representa
    /// dependencia.
    /// </summary>
    public record class EFIngredienteCantidad
    {
        /// <summary>
        /// Llave primaria
        /// </summary>
        [Key]
        public int Id {get; set;}
        /// <summary>
        /// Nombre del ingrediente (llave foránea).
        /// </summary>
        [ForeignKey(nameof(Ingrediente))]
        public string IngredienteNombre {get; set;}
        /// <summary>
        /// Referencia al ingrediente.
        /// </summary>
        public EFIngrediente Ingrediente {get; set;}

        /// <summary>
        /// Nombre del producto referenciado.
        /// </summary>
        [ForeignKey(nameof(Producto))]
        public string ProductoNombre {get; set;}
        /// <summary>
        /// Referencia al producto.
        /// </summary>
        public EFProducto Producto {get; set;} = null;
        
        /// <summary>Cantidad asociada al ingrediente.</summary>
        public double Cantidad {get; set;}

        /// <summary>Convertir a IngredienteCantidad</summary>
        public IngredienteCantidad Export()
            => new IngredienteEnStock {
                ingrediente = this.Ingrediente != null ? this.Ingrediente.Export() : null,
                cantidad = this.Cantidad,
            };
        
        public EFIngredienteCantidad(){}

        /// <summary>Construir basados en IngredienteCantidad</summary>
        public EFIngredienteCantidad(IngredienteCantidad ingredienteCantidad)
        {
            this.IngredienteNombre = ingredienteCantidad.ingrediente.nombre;
            this.Ingrediente = new EFIngrediente(ingredienteCantidad.ingrediente);
            this.Cantidad = ingredienteCantidad.cantidad;
        }

        public EFIngredienteCantidad(IngredienteEnStock ingredienteCantidad,
                                    EFContext ctx)
            : this(ingredienteCantidad)
        {
            this.Ingrediente = ctx.Ingredientes.Single(i => i.Nombre == this.IngredienteNombre);
        }
    }

    /// <summary>
    /// Representación de Producto para una base de datos soportada
    /// por EF.
    /// </summary>
    public record class EFProducto
    {
        /// <summary>Nombre del Producto</summary>
        [Key]
        public string Nombre {get; set;}
        /// <summary>Ingredientes que requiere el producto para ser
        /// preparado.</summary>
        public ICollection<EFIngredienteCantidad> IngredientesRequeridos { get; set; }
            = new List<EFIngredienteCantidad>();

        /// <summary>Convertir a Producto</summary>
        public Producto Export() => new Producto
        {
            nombre = this.Nombre,
            ingredientesRequeridos = (ICollection<IngredienteCantidad>)
                this.IngredientesRequeridos.Select(i => i.Export()).ToList()
        };
    }

    /// <summary>
    /// Permite relacionar una orden con los productos que ordena.
    /// Clave primaria compuesta: (NombreProducto, NumeroOrden)
    /// </summary>
    public record class EFProductoOrden
    {
        /// <summary>Nombre del producto que se ordena (Clave Foránea)</summary>
        [ForeignKey(nameof(Producto))]
        public string NombreProducto { get; set; }
        /// <summary>Número de orden a la que está asociada (Clave Foránea)</summary>
        [ForeignKey(nameof(Orden))]
        public int NumeroOrden { get; set; }
        /// <summary>Cantidad del producto ordenado.</summary>
        public int Cantidad { get; set; }

        /// <summary>Producto ordenado.</summary>
        public EFProducto Producto { get; set; }
        /// <summary>Orden asociada.</summary>
        public EFOrden Orden { get; set; }
    }

    /// <summary>
    /// Representación de Orden en bases de datos soportadas por EF.
    /// </summary>
    public record class EFOrden
    {
        /// <summary>Número de orden (clave primaria).</summary>
        [Key]
        public int NumeroOrden { get; set; }
        /// <summary>Estado de la orden.</summary>
        public EstadoOrden Estado { get; set; }
        /// <summary>Fecha de creación de la orden.</summary>
        public DateTime OrdenadaEn { get; set; }
        /// <summary>Productos que se ordenaron.</summary>
        public ICollection<EFProductoOrden> ProductosOrdenados { get; set; }
            = new List<EFProductoOrden>();

        /// <summary>Convertir a Orden.</summary>
        public Orden Export() => new Orden
        {
            numeroOrden = this.NumeroOrden,
            estado = this.Estado,
            ordenadaEn = this.OrdenadaEn,
            productosOrdenados = this.ProductosOrdenados.Select(po
                => (po.Producto.Export(), po.Cantidad)
            ).ToList()
        };

        public EFOrden(){}
        public EFOrden(Orden orden, EFContext ctx)
        {
            this.NumeroOrden = orden.numeroOrden;
            this.Estado = orden.estado;
            this.OrdenadaEn = orden.ordenadaEn;
            this.ProductosOrdenados = orden.productosOrdenados
                .Select(t => new EFProductoOrden
                {
                    NombreProducto = t.producto.nombre,
                    NumeroOrden = this.NumeroOrden,
                    Producto =  ctx.Productos
                        .Single(p => p.Nombre == t.producto.nombre),
                }).ToList();
        }
    }

}
