using System.Security.Cryptography;
using System.Text;

namespace Isas_Pizza
{
    /// <summary>
    /// Proveedor para cualquier objeto que implemente el hasheado de
    /// contraseñas.
    /// </summary>
    public interface IHashingProvider
    {
        public string Hash(string password);
        public bool Verify(string password, string hash);
    }

    public class SHA512HashingProvider : IHashingProvider
    {
        public string Hash(string password)
            => BitConverter.ToString(SHA512.Create()
                       .ComputeHash(Encoding.UTF8.GetBytes(password)))
                       .Replace("-", "")
                       .ToLower();

        public bool Verify(string password, string hash)
            => Hash(password).Equals(hash);
    }
}