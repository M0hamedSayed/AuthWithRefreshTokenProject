using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;
using Core.Features.Admin.Commands.Models;
using MediatR;
using Service.Interfaces;

namespace Core.Features.Admin.Commands.Handlers
{
    internal class AdminCommandHandler : ResponseHandler,
        IRequestHandler<DeleteUserQuery, Response<string>>,
        IRequestHandler<RestoreUserQuery, Response<string>>
    {
        private readonly IAdminService _adminService;

        public AdminCommandHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<Response<string>> Handle(DeleteUserQuery request, CancellationToken cancellationToken)
        {
            bool result =  await _adminService.HandleDeleteUser(request.Id);
            if (!result) return BadRequest<string>("User not found");
            return SuccessWithoutData<string>("This user is Deleted Successfully");
        }

        public async Task<Response<string>> Handle(RestoreUserQuery request, CancellationToken cancellationToken)
        {
            bool result = await _adminService.HandleRestoreUser(request.Id);
            if (!result) return BadRequest<string>("User not found");
            return SuccessWithoutData<string>("This user is Restored Successfully");
        }
    }
}
