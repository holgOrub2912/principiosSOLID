
namespace Isas_Pizza
{
    [Serializable]
    internal class InventarioInsuficienteException : Exception
    {
        public string nombreIngrediente;

        public InventarioInsuficienteException(string nombreIngrediente)
        {
            this.nombreIngrediente = nombreIngrediente;
        }
    }
}