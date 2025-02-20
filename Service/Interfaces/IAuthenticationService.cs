using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Identity;

namespace Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> CreateUserAsync(ApplicationUser user, string password);
        Task<UserRefreshTokens> GenerateTokensAsync(ApplicationUser user);

        Task<bool> ConfirmEmailAsync(int userId, string token);
        Task<string> ResendConfirmEmailAsync(string email);
        Task<string> RefreshTokenAsync();
        Task<bool> Logout();
    }
}
