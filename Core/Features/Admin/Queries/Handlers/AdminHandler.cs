using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Base;
using Core.Features.Admin.Queries.Models;
using Core.Features.Admin.Queries.Results;
using Data.Entities.Identity;
using MediatR;
using Service.Interfaces;

namespace Core.Features.Admin.Queries.Handlers
{
    public class AdminHandler : ResponseHandler,
        IRequestHandler<GetUserQuery, Response<GetUserResult>>
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        public AdminHandler(IAdminService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        public async Task<Response<GetUserResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _adminService.GetUserById(request.Id);
            if (user is null) return BadRequest<GetUserResult>(" User not found for this Id");

            GetUserResult result = _mapper.Map<GetUserResult>(user);
            return Success<GetUserResult>(result);
        }
    }
}
