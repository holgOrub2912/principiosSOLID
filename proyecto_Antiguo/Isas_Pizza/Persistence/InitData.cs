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
            new EFIngrediente("Pollo",
                              "Pechuga de pollo",
                              Unidad.GRAMO),
            new EFIngrediente("Salami",
                              "Salami en rodajas",
                              Unidad.GRAMO),
            new EFIngrediente("Piña",
                              "Piña en trozos",
                              Unidad.GRAMO),
            new EFIngrediente("Champiñones",
                              "Champiñones frescos",
                              Unidad.GRAMO),
            new EFIngrediente("Aceitunas",
                              "Aceitunas negras",
                              Unidad.GRAMO),
            new EFIngrediente("Pimiento",
                              "Pimiento rojo",
                              Unidad.UNIDAD),
            new EFIngrediente("Oregano",
                              "Oregano seco",
                              Unidad.GRAMO),
            new EFIngrediente("Aceite de oliva",
                              "Aceite de oliva virgen",
                              Unidad.MILILITRO),
            new EFIngrediente("Sal",
                              "Sal refinada",
                              Unidad.GRAMO),
            new EFIngrediente("Tocino",
                              "Tocino ahumado",
                              Unidad.GRAMO),
            new EFIngrediente("Carne molida",
                              "Carne de res molida",
                              Unidad.GRAMO),
            new EFIngrediente("Jamón",
                              "Jamón cocido",
                              Unidad.GRAMO),
            new EFIngrediente("Salsa de tomate",
                              "Salsa de tomate para pizza",
                              Unidad.MILILITRO)
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
                        Ingrediente = ingredientes[0], //tomate
                        Cantidad = 3
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[1], // Cebolla
                        Cantidad = 3
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[2], // Masa
                        Cantidad = 30.4
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[3], // Queso mozzarella
                        Cantidad = 200
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[19], // Salsa de tomate
                        Cantidad = 100
                    }
                }
            },
            new EFProducto
            {
                Nombre = "Paisa",
                Precio = 35000.0,
                IngredientesRequeridos = new EFIngredienteCantidad[]
                {
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[2], // Masa
                        Cantidad = 35
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[3], // Queso mozzarella
                        Cantidad = 250
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[7], // Pollo
                        Cantidad = 200
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[8], // Salami
                        Cantidad = 150
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[16], // Tocino
                        Cantidad = 100
                    }
                }
            },
            new EFProducto
            {
                Nombre = "Tradicional",
                Precio = 32000.0,
                IngredientesRequeridos = new EFIngredienteCantidad[]
                {
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[2], // Masa
                        Cantidad = 30
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[3], // Queso mozzarella
                        Cantidad = 300
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[4], // Peperoni
                        Cantidad = 200
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[19], // Salsa de tomate
                        Cantidad = 150
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[13], // Or�gano
                        Cantidad = 5
                    }
                }
            },
            new EFProducto
            {
                Nombre = "Hawaiana",
                Precio = 33000.0,
                IngredientesRequeridos = new EFIngredienteCantidad[]
                {
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[2], // Masa
                        Cantidad = 30
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[3],  // Queso mozzarella
                        Cantidad = 250
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[9], // Pi�a
                        Cantidad = 200
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[18], // Jam�n
                        Cantidad = 150
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[14], // Aceite de oliva
                        Cantidad = 10
                    }
                }
            },
            new EFProducto
            {
                Nombre = "Peperoni",
                Precio = 31000.0,
                IngredientesRequeridos = new EFIngredienteCantidad[]
                {
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[2], // Masa
                        Cantidad = 30
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[3], // Queso mozzarella
                        Cantidad = 300
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[4], // Peperoni
                        Cantidad = 250
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[19], // Salsa de tomate
                        Cantidad = 100
                    }
                }
            },
            new EFProducto
            {
                Nombre = "Carnes",
                Precio = 38000.0,
                IngredientesRequeridos = new EFIngredienteCantidad[]
                {
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[2], // Masa
                        Cantidad = 35
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[3], // Queso mozzarella
                        Cantidad = 250
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[4],  // Peperoni
                        Cantidad = 150
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[7], // Pollo
                        Cantidad = 150
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[17], // Carne molida
                        Cantidad = 150
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[16], // Tocino
                        Cantidad = 100
                    }
                }
            },
            new EFProducto
            {
                Nombre = "Queso",
                Precio = 29000.0,
                IngredientesRequeridos = new EFIngredienteCantidad[]
                {
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[2], // Masa
                        Cantidad = 30
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[3], // Queso mozzarella
                        Cantidad = 400
                    },
                    new EFIngredienteCantidad
                    {
                        Ingrediente = ingredientes[14], //Aceite de oliva
                        Cantidad = 10
                    }
                }
            }
        };

        public static EFIngredienteEnStock[] existencias = new EFIngredienteEnStock[]
        {
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[0],
                Cantidad = 30,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[1],
                Cantidad = 30,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[2],
                Cantidad = 10000,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[3],
                Cantidad = 1400,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[4],
                Cantidad = 50,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[5],
                Cantidad = 400,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[6],
                Cantidad = 600,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[7],
                Cantidad = 100,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[8],
                Cantidad = 800,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[9],
                Cantidad = 3000,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[10],
                Cantidad = 5000,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[11],
                Cantidad = 40,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[12],
                Cantidad = 10,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[13],
                Cantidad = 5600,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[14],
                Cantidad = 13400,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[15],
                Cantidad = 31000,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[16],
                Cantidad = 2300,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[17],
                Cantidad = 9000,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[18],
                Cantidad = 5000,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
            new EFIngredienteEnStock{
                Ingrediente = ingredientes[19],
                Cantidad = 4000,
                FechaVencimiento = DateTime.Today.AddDays(100)
            },
        };
    }
}
