using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerSetup.Infrastructure
{
    public interface IUserRepository
    {
        bool ValidateCredentials(string username, string password);

        User FindByUsername(string username);
    }
}
