using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Models;
using Microsoft.AspNetCore.Identity;

namespace LigaIsadoraSilva.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email); // Método para obter um usuário pelo email.

        Task<IdentityResult> AddUserAsync(User user, string password); // Método para adicionar um novo usuário.

        Task<SignInResult> LoginAsync(LoginViewModel model); // Método para realizar login.

        Task LogoutAsync(); // Método para realizar logout.

        Task<IdentityResult> UpdateUserAsync(User user); // Método para atualizar informações do usuário.

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword); // Método para alterar a senha do usuário.

        Task CheckRoleAsync(string roleName); // Método para verificar se uma função existe.

        Task AddUserToRoleAsync(User user, string roleName);  // Método para adicionar um usuário a uma função.

        Task<bool> IsUserInRoleAsync(User user, string roleName); // Método para verificar se o usuário está em uma função.

        Task<string> GenerateEmailConfirmationTokenAsync(User user); // Método para gerar um token de confirmação de email.

        Task<IdentityResult> ConfirmEmailAsync(User user, string token); // Método para confirmar o email do usuário.

        Task<string> GeneratePasswordResetTokenAsync(User user); // Método para gerar um token para redefinição de senha.

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password); // Método para redefinir a senha do usuário.
    }
}
