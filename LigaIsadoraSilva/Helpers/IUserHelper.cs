using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Models;
using Microsoft.AspNetCore.Identity;

namespace LigaIsadoraSilva.Helpers
{
    public interface IUserHelper
    {
        //Task CheckRoleAsync(string roleName);

        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task AddUserToRoleAsync(User user, string roleName);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    }
}