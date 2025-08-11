using Isas_Pizza;

namespace Isas_Pizza.Persistence
{
    public static class InitData
    {
        public static EFIngrediente[] ingredientes = new EFIngrediente[]
        {
            new EFIngrediente("Tomate",
                              "Tomate Alino",
                              Unidad.UNIDAD),
            new EFIngrediente("Ajo",
                              "Diente de ajo",
                              Unidad.UNIDAD),
            new EFIngrediente("Masa",
                              "Masa de trigo",
                              Unidad.GRAMO),
            new EFIngrediente("pimienta",
                              "Pimienta negra molida",
                              Unidad.GRAMO),
        };
    }
}
