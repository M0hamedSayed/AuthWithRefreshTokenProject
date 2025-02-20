using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(user => user.userRefreshTokens)
                .WithOne(token => token.user)
                .HasForeignKey(token => token.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
