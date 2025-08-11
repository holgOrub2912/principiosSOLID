using System;
using Isas_Pizza;

namespace Isas_Pizza.Persistence
{
    public class AuthPersistenceLayer : IROPersistenceLayer<RegisteredUser>
    {
        private RegisteredUser[] _users;

        public AuthPersistenceLayer(string authFilePath)
        {
            this._users = File
                .ReadAllLines(authFilePath)
                .Select(l => l.Split(";"))
                .Select(a => new RegisteredUser(int.Parse(a[0]), a[1], Enum.Parse<UserRole>(a[2])))
                .ToArray();
        }
        IEnumerable<RegisteredUser> IROPersistenceLayer<RegisteredUser>.View(RegisteredUser? _)
            => this._users;
    }
}