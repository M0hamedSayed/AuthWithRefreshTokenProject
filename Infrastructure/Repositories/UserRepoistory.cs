using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Identity;
using Infrastructure.Base;
using Infrastructure.DatabaseContext;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepoistory : GenericRepositoryAsync<ApplicationUser>,IUserRepository
    {
        private readonly DbSet<ApplicationUser> _users;

        public UserRepoistory(ApplicationDbContext context) : base(context)
        {
            _users = context.Set<ApplicationUser>();
        }
    }
}
