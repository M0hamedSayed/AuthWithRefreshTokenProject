
using Core.Features.User.Queries.Results;
using Data.Entities.Identity;

namespace Core.Mapping.User
{
    public partial class UserProfile
    {
        public void UserMapping()
        {
            CreateMap<UserRefreshTokens, ActiveUserSession>();

            CreateMap<ApplicationUser, GetMeResults>()
                .ForMember(dest => dest.activeSessions, opt => opt.MapFrom(src => src.userRefreshTokens));
        }
    }
}
