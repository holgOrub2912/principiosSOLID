using System.Reflection;

namespace Isas_Pizza.UserMenus
{
    /// <summary>
    /// Representa un conjunto de acciones que un usuario puede realizar.
    /// </summary>
    public class UserMenu
    {
        /// <summary>
        /// Generar opciones de menú a partir de los métodos de la clase actual.
        /// </summary>
        /// <returns>Lista de opciones para el menú actual.</returns>
        public IEnumerable<(string, Action<Pizzeria>)> Menu()
            => this.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(method => Attribute.IsDefined(
                    method, typeof(MenuOptionAttribute)
                ))
                .Select(method => (
                    ((MenuOptionAttribute)
                     method.GetCustomAttribute(typeof(MenuOptionAttribute))
                    ).label,
                    (Action<Pizzeria>)
                        method.CreateDelegate(typeof(Action<Pizzeria>))
                ));
        
    }
}