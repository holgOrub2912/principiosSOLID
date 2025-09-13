using System;
using Isas_Pizza.UserMenus;

namespace Isas_Pizza
{
    public enum UserRole
    {
        CONSUMIDOR,
        ADMINISTRADOR,
        CHEF,
    }

    public static class UserRoleExtensions
    {
        public static UserMenu GetMenu(this UserRole rol)
            => rol switch
            {
                UserRole.CONSUMIDOR => new ConsumidorMenu(),
                UserRole.ADMINISTRADOR => new AdministradorMenu(),
                UserRole.CHEF => new ChefMenu(),
            };
    }

    /// <summary>
    /// Representación de un usuario activo en la sesión.
    /// </summary>
    public interface IUserAgent
    {
        /// <summary>
        /// Obtener el rol del usuario.
        /// </summary>
        /// returns rol del usuario.
        public UserRole GetRole();
        /// <summary>
        /// Obtener un menú para este usuario
        /// </summary>
        /// returns UserMenu del usuario.
        public UserMenu GetMenu();
    }

    /// <summary>
    /// Usuario registrado (con privilegios especiales) dentro del
    /// sistema.
    /// </summary>
    public record class RegisteredUser(int id, string passwordHash, UserRole rol) : IUserAgent
    {
        private UserMenu _menu = rol.GetMenu();
        public UserRole GetRole() => this.rol;
        public UserMenu GetMenu() => _menu;
    }

    /// <summary>
    /// Usuario no registrado (consumidor) dentro del sistema.
    /// </summary>
    public record class NonRegisteredUser: IUserAgent
    {
        private UserMenu _menu = UserRole.CONSUMIDOR.GetMenu();
        public UserRole GetRole() => UserRole.CONSUMIDOR;
        public UserMenu GetMenu() => _menu;
    }

}
