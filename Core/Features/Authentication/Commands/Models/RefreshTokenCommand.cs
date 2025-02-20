using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;
using Core.Features.Authentication.Commands.Results;
using MediatR;

namespace Core.Features.Authentication.Commands.Models
{
    public class RefreshTokenCommand: IRequest<Response<RefreshTokenResult>>
    {
    }
}
