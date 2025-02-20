using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Identity;
using Infrastructure.DatabaseContext;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager) : IUnitOfWork
    {
        private readonly ApplicationDbContext _Context  = context;

        public IUserRepository UserRepoistory { get; } = new UserRepoistory(context);

        public IRefreshTokensRepository RefreshTokensRepository { get; } = new RefreshTokensRepository(context);

        public UserManager<ApplicationUser> UserManager { get; } = userManager;

        public SignInManager<ApplicationUser> SignInManager { get; } = signInManager;

        public RoleManager<ApplicationRole> RoleManager { get; } = roleManager;

        public async Task Complete()
        {
            await _Context.SaveChangesAsync();
        }

        public void Dispose() => _Context.Dispose();
    }
}
