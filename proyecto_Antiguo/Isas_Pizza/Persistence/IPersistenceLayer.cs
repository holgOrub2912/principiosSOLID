namespace Isas_Pizza.Persistence
{
    /// <summary>
    /// Representa cualquier implementación de una capa de persistencia
    /// de sólo lectura para el tipo de dato T
    /// </summary>
    public interface IROPersistenceLayer<T>
    {
        public IEnumerable<T> View(T? _);
    }

    /// <summary>
    /// Representa cualquier implementación de una capa de persistencia
    /// para el tipo de dato T
    /// </summary>
    public interface IPersistenceLayer<T> : IROPersistenceLayer<T>
    {
        public void Save(IEnumerable<T> entities);
        public void Delete(T entity);
        public void Update(T target, T newEntity);
    }
}
