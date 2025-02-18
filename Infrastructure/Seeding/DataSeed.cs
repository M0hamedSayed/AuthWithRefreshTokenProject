using Data.Entities.Identity;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeding
{
    public static class DataSeed
    {
        public static async Task SeedAsync(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) return;

            int rolesCount = await unitOfWork.RoleManager.Roles.CountAsync();
            if (rolesCount <= 0)
            {

                await unitOfWork.RoleManager.CreateAsync(new ApplicationRole()
                {
                    Name = "Admin"
                });
                await unitOfWork.RoleManager.CreateAsync(new ApplicationRole()
                {
                    Name = "User"
                });
            }

            var usersCount = await unitOfWork.UserManager.Users.CountAsync();
            if (usersCount <= 0)
            {
                var defaultuser = new ApplicationUser()
                {
                    UserName = "admin@auth.com",
                    Email = "admin@auth.com",
                    FullName = "Mohamed Sayed",
                    PhoneNumber = "123456",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await unitOfWork.UserManager.CreateAsync(defaultuser, "M123_m");
                await unitOfWork.UserManager.AddToRoleAsync(defaultuser, "Admin");
            }
        }
    }
}
