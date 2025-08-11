using System;

namespace Isas_Pizza
{
    public enum UserRole
    {
        CONSUMIDOR,
        ADMINISTRADOR,
        CHEF,
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
    }

    /// <summary>
    /// Usuario registrado (con privilegios especiales) dentro del
    /// sistema.
    /// </summary>
    public record class RegisteredUser(int id, string passwordHash, UserRole rol) : IUserAgent
    {
        public UserRole GetRole() => this.rol;
    }

    /// <summary>
    /// Usuario no registrado (consumidor) dentro del sistema.
    /// </summary>
    public record class NonRegisteredUser: IUserAgent
    {
        public UserRole GetRole() => UserRole.CONSUMIDOR;
    }

}
