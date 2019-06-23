using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace IdentityServerSetup.Infrastructure
{
    public class ResourceOwnerValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository userRepository;
        public ResourceOwnerValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //در این جا مشخص میشود لاگین انجام شده یا نه
            if (userRepository.ValidateCredentials(context.UserName, context.Password))
            {
                //شناسایی شد
                context.Result = new GrantValidationResult("1", "custom");
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            }
        }
    }
}
