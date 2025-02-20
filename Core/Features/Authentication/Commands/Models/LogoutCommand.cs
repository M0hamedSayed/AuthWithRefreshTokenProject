using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;
using MediatR;

namespace Core.Features.Authentication.Commands.Models
{
    public class LogoutCommand:IRequest<Response<string>>
    {
    }
}
