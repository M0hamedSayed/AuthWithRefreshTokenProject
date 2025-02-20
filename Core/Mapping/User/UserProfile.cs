using AutoMapper;

namespace Core.Mapping.User
{
    public partial class UserProfile : Profile
    {
        public UserProfile() 
        {
            UserMapping();
        }
    }
}
