
using Data.Entities.Identity;
using Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork :IDisposable
    {
        IUserRepository UserRepoistory { get; }
        IRefreshTokensRepository RefreshTokensRepository { get; }
        UserManager<ApplicationUser> UserManager { get; }
        SignInManager<ApplicationUser> SignInManager { get; }
        RoleManager<ApplicationRole> RoleManager { get; }
        void Complete();

    }
}
