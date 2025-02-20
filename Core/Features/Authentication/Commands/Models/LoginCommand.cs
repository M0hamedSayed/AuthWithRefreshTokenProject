using Core.Base;
using Core.Features.Authentication.Commands.Results;
using MediatR;

namespace Core.Features.Authentication.Commands.Models
{
    public class LoginCommand : IRequest<Response<LoginResult>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
