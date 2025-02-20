using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Features.Authentication.Commands.Models;
using Data.Entities.Identity;

namespace Core.Mapping.Authentication
{
    public partial class AuthenticationProfile
    {
        public void CreateUserMapping()
        {
            CreateMap<CreateUserCommand,ApplicationUser>()
                .ForMember( dest => dest.UserName, opt => opt.MapFrom( src => src.Email ) );
        }
    }
}
