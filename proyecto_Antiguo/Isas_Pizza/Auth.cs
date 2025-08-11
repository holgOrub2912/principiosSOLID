using System;
using Isas_Pizza.IO;
using Isas_Pizza.Persistence;

namespace Isas_Pizza
{
    /// <summary>
    /// Credenciales de autenticación de un usuario.
    /// </summary>
    /// \todo Poner validaciones apropiadas.
    public record struct LoginCredentials
    {
        public int id;
        public string password;
    }
    /// <summary>
    /// Representa un objeto capaz de entregar un usuario autenticado dado
    /// un medio para preguntarle al usuario sus credenciales.
    /// </summary>
    /// \todo Implementar esta interfaz, junto con una implementación
    ///       para IO.IBlockingPrompter<LoginCredentials>
    public interface IAuthenticator
    {
        /// <summary>
        /// Preguntar por los credenciales de usuario y retornar un
        /// IUserAgent que represente un usuario que coincida con estos
        /// credenciales.
        /// </summary>
        /// <param name="credentialPrompter">Prompter de credenciales
        /// de acceso, donde Ask(LoginCredentials? _) puede retornar
        /// nulo cuando el usuario quiere usar la aplicación como
        /// consumidor.</param>
        public IUserAgent Authenticate(
            IBlockingPrompter<LoginCredentials?> credentialPrompter
        );
    }

    /// <summary>
    /// Implementación del IAuthenticator que utiliza una
    /// IROPersistenceLayer<RegisteredUser> para obtener los
    /// registros de autenticación.
    /// </summary>
    /// \todo Implementar la interfaz apropiadamente, junto con una
    ///       implementación simple de
    ///       IROPersistenceLayer<RegisteredUser> (puede ser de un
    ///       archivo de texto)
    public class GenericAuthenticator: IAuthenticator
    {
        private IROPersistenceLayer<RegisteredUser> _userRegister;
        private IBlockingDisplayer<string> _printer;

        public GenericAuthenticator(
            IROPersistenceLayer<RegisteredUser> userRegister,
            IBlockingDisplayer<string> printer
        )
        {
            this._userRegister = userRegister;
            this._printer = printer;
        }

        public IUserAgent Authenticate(
            IBlockingPrompter<LoginCredentials?> prompter
        )
        {
            throw new NotImplementedException();
        }
    }
}
