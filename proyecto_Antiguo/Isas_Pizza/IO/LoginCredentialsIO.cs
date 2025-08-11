namespace Isas_Pizza.IO
{
    public class CredentialPrompter : IBlockingPrompter<LoginCredentials?>
    {
        public LoginCredentials? Ask(LoginCredentials? _)
        {
            string input;
            do 
                Console.Write("Quieres iniciar sesión? (y/n) ");
            while(
                (input = Console.ReadLine().ToLower()).Length == 0 || 
                (input[0] != 'y' && input[0] != 'n')
            );

            if (input[0] == 'n')
                return null;

            int id;
            string password;
            do
                Console.Write("Id: ");
            while (!int.TryParse(Console.ReadLine(), out id));
            
            /// \todo Esconder contraseña como en el programa original?
            do
                Console.Write("Contraseña: ");
            while ((password = Console.ReadLine()).Length == 0);

            return new LoginCredentials{id = id, password = password};
        }
    }
}