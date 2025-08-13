using Isas_Pizza;
using Isas_Pizza.Persistence.EFModel;

namespace Isas_Pizza.Persistence
{
    public static class InitData
    {
        public static EFIngrediente[] ingredientes = new EFIngrediente[]
        {
            new EFIngrediente("Tomate",
                              "Tomate Alino",
                              Unidad.UNIDAD),
            new EFIngrediente("Cebolla",
                              "Unidad de cebolla",
                              Unidad.UNIDAD),
            new EFIngrediente("Masa",
                              "Masa de trigo",
                              Unidad.GRAMO),
            new EFIngrediente("Queso mozzarella",
                              "Queso mozzarella en bloque",
                              Unidad.GRAMO),
            new EFIngrediente("Peperoni",
                              "Peperoni en torrejas",
                              Unidad.GRAMO),
            new EFIngrediente("Ajo",
                              "Diente de ajo",
                              Unidad.UNIDAD),
            new EFIngrediente("pimienta",
                              "Pimienta negra molida",
                              Unidad.GRAMO),
        };

        public static EFProducto[] productos = new EFProducto[]
        {
            new EFProducto
            {
                Nombre = "Napolitana",
                Precio = 30000.0,
                IngredientesRequeridos = new EFIngredienteCantidad[]
                {
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[0],
                        Cantidad = 3
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[1],
                        Cantidad = 3
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[2],
                        Cantidad = 30.4
                    },
                }
            }
        };
    }
}
