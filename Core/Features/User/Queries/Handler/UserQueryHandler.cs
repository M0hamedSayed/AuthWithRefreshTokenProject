using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Base;
using Core.Features.Admin.Queries.Results;
using Core.Features.User.Queries.Models;
using Core.Features.User.Queries.Results;
using Data.Entities.Identity;
using MediatR;
using Service.Interfaces;

namespace Core.Features.User.Queries.Handler
{
    public class UserQueryHandler : ResponseHandler,
        IRequestHandler<GetMeQuery, Response<GetMeResults>>
    {
        private readonly IAdminService _adminService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UserQueryHandler(IAdminService adminService, ICurrentUserService currentUserService,IMapper mapper)
        {
            _adminService = adminService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Response<GetMeResults>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            ApplicationUser? user = await _adminService.GetUserById(userId);

            if (user is null) return BadRequest<GetMeResults>("Something wrong");

            GetMeResults result = _mapper.Map<GetMeResults>(user);
            return Success<GetMeResults>(result);
        }
    }
}
