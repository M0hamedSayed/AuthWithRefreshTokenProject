using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Identity;

namespace Service.Interfaces
{
    public interface IAdminService
    {
        Task<ApplicationUser?> GetUserById(int id);
        Task<bool> HandleDeleteUser(int id);

        Task<bool> HandleRestoreUser(int id);
    }
}
