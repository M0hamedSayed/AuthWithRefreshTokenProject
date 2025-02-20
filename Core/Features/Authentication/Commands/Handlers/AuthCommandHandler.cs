using AutoMapper;
using Core.Base;
using Core.Features.Authentication.Commands.Models;
using Core.Features.Authentication.Commands.Results;
using Data.Entities.Identity;
using Infrastructure.Interfaces;
using MediatR;
using Service.Interfaces;

namespace Core.Features.Authentication.Commands.Handlers
{
    internal class AuthCommandHandler : ResponseHandler,
        IRequestHandler<CreateUserCommand, Response<string>>,
        IRequestHandler<LoginCommand, Response<LoginResult>>,
        IRequestHandler<RefreshTokenCommand, Response<RefreshTokenResult>>,
        IRequestHandler<LogoutCommand, Response<string>>
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public AuthCommandHandler(IMapper mapper, IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        // register
        public async Task<Response<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var IdentityUser = _mapper.Map<ApplicationUser>(request);

            string createResult = await _authenticationService.CreateUserAsync(IdentityUser, request.Password);

            switch(createResult)
            {
                case "ErrorInCreateUser": return BadRequest<string>("Failed To Add USer.");
                case "Failed": return BadRequest<string>("Operation Is Failed, Try To Register Again");
                case "EmailFailed": return BadRequest<string>("Email confirmation not sent successfully, please resend confirmation email");
                case "Success": return SuccessWithoutData<string>("Registeration Success. Please Check your Mail");
                default: return BadRequest<string>(createResult);
            }
        }


        // login
        public async Task<Response<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // check if user exist
            var user = await _unitOfWork.UserManager.FindByEmailAsync(request.Email);
            if (user is null) return BadRequest<LoginResult>("Email or passwrod wrong");
            if (user.IsDeleted) return BadRequest<LoginResult>("Your account is deactivated please contact with support team");

            // check email confirmation
            if (!user.EmailConfirmed) return BadRequest<LoginResult>("Please confirm your email");
            // validate password
            var userSignIn = await _unitOfWork.SignInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!userSignIn.Succeeded) return BadRequest<LoginResult>("Email or passwrod wrong");
            // generate token and refresh token
            var userRefreshToken = await _authenticationService.GenerateTokensAsync(user);

            var loginResult = _mapper.Map<LoginResult>(user, opts =>
            {
                opts.Items["refreshToken"] = userRefreshToken;
            });

            return Success<LoginResult>(loginResult);
        }

        public async Task<Response<RefreshTokenResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _authenticationService.RefreshTokenAsync();
            return Success<RefreshTokenResult>( new() { Token = token });
        }

        public async Task<Response<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            bool isLoggedOut = await _authenticationService.Logout();
            if (!isLoggedOut) return BadRequest<string>("You already Loggedout");
            return SuccessWithoutData<string>();
        }
        #endregion

    }
}
