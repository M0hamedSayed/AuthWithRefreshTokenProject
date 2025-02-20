using Api.Base;
using Core.Features.Authentication.Commands.Models;
using Core.Features.Authentication.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [AllowAnonymous]
    public class AuthController : AppControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var res = await Mediator.Send(command);
            return HandleResult(res);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginCommand command)
        {
            var res = await Mediator.Send(command);
            return HandleResult(res);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery command)
        {
            var res = await Mediator.Send(command);
            return HandleResult(res);
        }

        [HttpPost("send-confirm-email")]
        public async Task<IActionResult> SignIn([FromBody] ResendConfirmEmailQuery command)
        {
            var res = await Mediator.Send(command);
            return HandleResult(res);
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var res = await Mediator.Send(new RefreshTokenCommand());
            return HandleResult(res);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var res = await Mediator.Send(new LogoutCommand());
            return HandleResult(res);
        }
    }
}
