using Data.Entities.Identity;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApplicationUser?> GetUserById(int id)
        {
            ApplicationUser? user = await _unitOfWork.UserRepoistory
                .GetTableNoTracking()
                .IgnoreQueryFilters()
                .Include(user => user.userRefreshTokens)
                .Where(user => user.Id == id)
                .SingleOrDefaultAsync();
            return user;
        }

        public async Task<bool> HandleRestoreUser(int id)
        {
            //get user
            var user = await _unitOfWork.UserRepoistory.GetTableAsTracking().IgnoreQueryFilters().Where(user => user.Id == id).SingleOrDefaultAsync();
            if (user is null) return false;
            user.IsDeleted = false;
            user.DeletedAt = null;
            // save changes
            await _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> HandleDeleteUser(int id)
        {
            // don't delete first admin user
            if (id == 1) return false;
            //get user
            var user = await _unitOfWork.UserRepoistory.GetByIdAsync(id);
            if (user is null) return false;
            // delete user
            _unitOfWork.UserRepoistory.Delete(user);
            // save changes
            await _unitOfWork.Complete();
            return true;
        }
    }   
}
