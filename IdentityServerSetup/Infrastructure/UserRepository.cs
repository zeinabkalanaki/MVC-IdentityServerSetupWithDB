using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerSetup.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        public User FindByUsername(string username)
        {
            return new User()
            {
                SubjectId = "1",
                Username = "Reyhaneh"
            };
        }

        public bool ValidateCredentials(string username, string password)
        {
            if (username.ToLower() == "reyhaneh" && password == "p@ssWord")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
