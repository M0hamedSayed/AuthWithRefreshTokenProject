
using Core.Base;
using Core.Features.Authentication.Queries.Models;
using MediatR;
using Service.Interfaces;

namespace Core.Features.Authentication.Queries.Handlers
{
    public class AuthQueryHandler : ResponseHandler,
        IRequestHandler<ConfirmEmailQuery, Response<string>>,
        IRequestHandler<ResendConfirmEmailQuery, Response<string>>
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthQueryHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async Task<Response<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var response = await _authenticationService.ConfirmEmailAsync(request.userId, request.token);
            if(!response) return BadRequest<string>("Something error happen when confirm email");
            return SuccessWithoutData<string>("Email is confirmed successfully");

        }

        public async Task<Response<string>> Handle(ResendConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var response = await _authenticationService.ResendConfirmEmailAsync(request.Email);
            if(response!= "Success") return BadRequest<string>(response);
            return SuccessWithoutData<string>("ReConfime-Email is sent successfully");
        }
    }
}
