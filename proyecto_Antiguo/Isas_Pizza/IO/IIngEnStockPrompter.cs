namespace Isas_Pizza
{
    public interface IIngEnStockPrompter
    {
        IngredienteEnStock Ask(Ingrediente ingrediente);
        IngredienteEnStock Ask(IngredienteEnStock ingrediente);
    }
}