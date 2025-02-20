using Api.Base;
using Core.Features.Admin.Commands.Models;
using Core.Features.Admin.Queries.Models;
using Core.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Authorize(Roles = "Admin")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class AdminController : AppControllerBase
    {
        [HttpPost("user/{Id}")]
        public async Task<IActionResult> GetUser([FromRoute] int Id)
        {
            var res = await Mediator.Send(new GetUserQuery(Id));
            return HandleResult(res);
        }

        [HttpPost("delete-user/{Id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int Id)
        {
            var res = await Mediator.Send(new DeleteUserQuery(Id));
            return HandleResult(res);
        }

        [HttpPost("restore-user/{Id}")]
        public async Task<IActionResult> RestoreUser([FromRoute] int Id)
        {
            var res = await Mediator.Send(new RestoreUserQuery(Id));
            return HandleResult(res);
        }
    }
}
