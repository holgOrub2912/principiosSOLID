namespace Isas_Pizza {
    public interface ICounterVisitor<T>
    {
        public T Visit(Orden orden);
        public T Visit(Orden orden, int cantidad);
        public T Visit(Producto producto, int cantidad);
        public T Visit(Ingrediente ingrediente, double cantidad);
    }
}