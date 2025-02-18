using Data.Entities.Identity;
using Infrastructure.Base;
using Infrastructure.DatabaseContext;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RefreshTokensRepository : GenericRepositoryAsync<UserRefreshTokens>, IRefreshTokensRepository
    {
        private readonly DbSet<UserRefreshTokens> _refreshTokens;

        public RefreshTokensRepository(ApplicationDbContext context) : base(context)
        {
            _refreshTokens = context.Set<UserRefreshTokens>();
        }
    }
}
