using System;
using Isas_Pizza.IO;
using Isas_Pizza.Persistence;

namespace Isas_Pizza
{
    /// <summary>
    /// Credenciales de autenticación de un usuario.
    /// </summary>
    public record struct LoginCredentials(int id, string password);
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
        private IHashingProvider _hasher;

        public GenericAuthenticator(
            IROPersistenceLayer<RegisteredUser> userRegister,
            IBlockingDisplayer<string> printer
        )
        {
            this._userRegister = userRegister;
            this._printer = printer;
            this._hasher = new SHA512HashingProvider();
        }

        public IUserAgent Authenticate(
            IBlockingPrompter<LoginCredentials?> prompter
        )
        {
            LoginCredentials? credentials = prompter.Ask(null);
            if (credentials is null)
                return new NonRegisteredUser();
            
            RegisteredUser? usuario = _userRegister.View(null).FirstOrDefault(
                u => u.id == credentials?.id &&
                    _hasher.Verify(credentials?.password, u.passwordHash)
            );
            if (usuario is not null)
                return usuario;
            this._printer.Display(new string[]{"Credenciales inválidas."});
            return this.Authenticate(prompter);
        }
    }
}
