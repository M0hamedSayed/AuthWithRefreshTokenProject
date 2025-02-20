using Core.Features.Admin.Queries.Results;
using Data.Entities.Identity;

namespace Core.Mapping.Admin
{
    public partial class AdminProfile
    {
        public void UserMapping()
        {
            CreateMap<UserRefreshTokens, ActiveUserSession>();

            CreateMap<ApplicationUser, GetUserResult>()
                .ForMember(dest => dest.activeSessions, opt => opt.MapFrom( src => src.userRefreshTokens));
        }
    }
}
