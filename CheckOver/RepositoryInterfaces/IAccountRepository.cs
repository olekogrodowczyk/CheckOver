using CheckOver.Models;
using CheckOver.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> ChangePassword(ChangePasswordVM changePasswordVM);

        Task<IdentityResult> CreateUserAsync(SignUpVM userModel);

        Task<SignInResult> PasswordSignInAsync(SignInVM signInModel);

        Task SignOutAsync();
    }
}