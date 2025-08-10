namespace Isas_Pizza.IO
{
    /// \todo Implementar esta interfaz
    /// <summary>
    /// Representa un objeto que provee un menú genérico en una
    /// interfaz bloqueante. e.g. la consola.
    /// </summary>
    public interface IBlockingSelector
    {
        /// <summary>
        /// Preguntarle al usuario cuál de las opciones seleccionar
        /// </summary>
        /// <param name="options">Coleccion de tuplas de la forma
        /// (etiqueta, opcion)</param>
        /// <returns>Opción seleccionada</returns>
        public T SelectOne<T>(ICollection<(string label, T option)> options);
    }

    /// \todo Implementar esta interfaz para Orden, IngredienteEnStock y Producto
    /// <summary>
    /// Representa cualquier objeto capaz de mostrarle al usuario una
    /// colección de objetos de tipo T en una interfaz
    /// bloqueante. e.g. la consola.
    /// </summary>
    public interface IBlockingDisplayer<T>
    {
        /// <summary>
        /// Mostrar al usuario los elementos en elements.
        /// </summary>
        /// <param name="elements">Elementos a mostrar.</param>
        public void Display<T>(ICollection<T> elements);
    }

    /// \todo Implementar esta interfaz para Orden e IngredienteEnStock
    /// <summary>
    /// Representa cualquier objeto capaz de preguntarle al usuario por
    /// un objeto T en una interfaz
    /// bloqueante. e.g. la consola.
    /// </summary>
    public interface IBlockingPrompter<T>
    {
        /// <summary>
        /// Mostrar al usuario los elementos en elements.
        /// </summary>
        /// <returns>Objeto surgido de la entrada del usuario.</returns>
        public T Ask<T>();
    }
}
