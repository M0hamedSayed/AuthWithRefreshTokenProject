using AutoMapper;

namespace Core.Mapping.Authentication
{
    public partial class AuthenticationProfile: Profile
    {
        public AuthenticationProfile()
        {
            CreateUserMapping();
            LoginMapping();
        }
    }
}
