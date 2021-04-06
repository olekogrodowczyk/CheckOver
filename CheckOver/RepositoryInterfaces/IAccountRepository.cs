using CheckOver.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpVM userModel);

        Task<SignInResult> PasswordSignInAsync(SignInVM signInModel);

        Task SignOutAsync();
    }
}