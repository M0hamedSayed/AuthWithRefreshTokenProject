using Core.Features.Authentication.Commands.Results;
using Data.Entities.Identity;

namespace Core.Mapping.Authentication
{
    public partial class AuthenticationProfile
    {
        public void LoginMapping()
        {
            CreateMap<UserRefreshTokens, ActiveSession>();

            CreateMap<ApplicationUser, LoginResult>()
                .ForMember(dest => dest.activeSession, opt => opt.MapFrom((src, dest, destMember, context) =>
                context.Mapper.Map<ActiveSession>(context.Items["refreshToken"])));
        }
    }
}
