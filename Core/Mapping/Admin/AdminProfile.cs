using AutoMapper;

namespace Core.Mapping.Admin
{
    public partial class AdminProfile :Profile
    {
        public AdminProfile()
        {
            UserMapping();
        }
    }
}
