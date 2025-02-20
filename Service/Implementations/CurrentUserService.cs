using Data.Entities.Identity;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;

namespace Service.Implementations
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public CurrentUserService(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork) 
        {
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<string>> GetCurrentUserRolesAsync()
        {
            var user = await GetUserAsync();
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
            return [.. roles];
        }

        public async Task<ApplicationUser> GetUserAsync()
        {
            var userId = GetUserId();
            var user = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
            return user is null ? throw new UnauthorizedAccessException() : user;
        }

        public int GetUserId()
        {
            var userId = _contextAccessor?.HttpContext?.User?.Claims?.SingleOrDefault(claim => claim.Type == "Id")?.Value;
            
            if (userId == null || !int.TryParse(userId, out var id))
            {
                throw new UnauthorizedAccessException();
            }
            return id;
        }
    }
}
