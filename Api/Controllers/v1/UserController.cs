using Api.Base;
using Core.Features.User.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Authorize(Roles = "Admin,User")]
    public class UserController : AppControllerBase
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetUser()
        {
            var res = await Mediator.Send(new GetMeQuery());
            return HandleResult(res);
        }
    }
}
