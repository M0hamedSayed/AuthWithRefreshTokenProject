using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Data.Entities.Identity;
using Data.Helpers;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Service.Helpers;
using Service.Interfaces;

namespace Service.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailService _emailService;
        private readonly TokenSettings _tokenSettings;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ICurrentUserService _currentUserService;

        


        public AuthenticationService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor,IEmailService emailService, TokenSettings tokenSettings, IHostEnvironment hostEnvironment,ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _emailService = emailService;
            _tokenSettings = tokenSettings;
            _hostEnvironment = hostEnvironment;
            _currentUserService = currentUserService;
        }

        public async Task<string> CreateUserAsync(ApplicationUser user, string password)
        {
            string addingUserResult = await AddUserWithRoleAsync(user, password);
            if (addingUserResult != "Success")
            {
                return addingUserResult;
            }

            bool sendEmail = await generateTokenAndSendConfirmEmailAsync(user);
            if (!sendEmail) return "EmailFailed";
            return "Success";
        }

        public async Task<UserRefreshTokens> GenerateTokensAsync(ApplicationUser user)
        {
            string accessToken = await GenerateGwtToken(user);
            string refreshToken = GenerateRefreshToken();

            // get user ip address
            string? userIp = GetUserIp();
            if (_hostEnvironment.IsDevelopment())
            {
                userIp = "41.46.58.182";
            }
            GeoLocation? userMetaData = null;

            if (userIp is not null)
            {
                userMetaData = await GetLocationFromIP(userIp);
            }

            UserRefreshTokens userRefreshToken = new UserRefreshTokens
            { 
                UserId = user.Id,
                AccessToken = accessToken, 
                RefreshToken = refreshToken,
                AddedTime = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpireDate),
                IsUsed = true,
                IP = userIp,
                UserAgent = GetUserAgent(),
                Country = userMetaData?.country,
                CountryCode = userMetaData?.countryCode,
                City = userMetaData?.city,
                LocationLat = userMetaData?.lat,
                LocationLng = userMetaData?.lon,
                TimeZone = userMetaData?.timezone,
            };
            // save refresh token
            await _unitOfWork.RefreshTokensRepository.AddAsync(userRefreshToken);
            await _unitOfWork.Complete();
            // add refreshtoken to cookies
            _contextAccessor?.HttpContext?.Response.Cookies.Append(
                "refreshToken", 
                refreshToken,
                options: new CookieOptions { Expires = userRefreshToken.ExpiryDate, HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict }
            );
            return userRefreshToken;
        }

        public async Task<bool> ConfirmEmailAsync(int userId,  string token)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;
            var confrimEmail = await _unitOfWork.UserManager.ConfirmEmailAsync(user, token);
            if(!confrimEmail.Succeeded) return false;
            return true;
        }

        public async Task<string> RefreshTokenAsync()
        {
            var accessToken = _contextAccessor?.HttpContext?.Request.Headers?.Authorization.SingleOrDefault()?.Split(" ")[1];
            var refreshtoken = _contextAccessor?.HttpContext?.Request?.Cookies["refreshToken"];
            if (accessToken is null || refreshtoken is null) throw new UnauthorizedAccessException("Invalid Token");

            var principal = GetPrincipalFromJwtToken(accessToken);

            if (principal is null) throw new UnauthorizedAccessException("Invalid Token");

            string? id = principal.FindFirstValue("Id");
            var user = await _currentUserService.GetUserAsync();
            if (user is null || user.IsDeleted)
                throw new UnauthorizedAccessException("Invalid User");
            // get refresh token
            var rToken = await _unitOfWork.RefreshTokensRepository.FindAsync(rt => rt.RefreshToken == refreshtoken, new[] { "user" });

            if (rToken is null || rToken.IsRevoked)
            {
                _contextAccessor?.HttpContext?.Response.Cookies.Delete("refreshToken");
                throw new UnauthorizedAccessException("Invalid Refresh Token");
            }

            if (rToken.ExpiryDate <= rToken.AddedTime)
            {
                rToken.IsRevoked = true;
                await _unitOfWork.Complete();
                _contextAccessor?.HttpContext?.Response.Cookies.Delete("refreshToken");
                throw new UnauthorizedAccessException("Invalid Refresh Token");
            }

            var newAccessToken = await GenerateGwtToken(user);

            rToken.AccessToken = newAccessToken;
            await _unitOfWork.Complete();
            
            return newAccessToken;

        }

        public async Task<bool> Logout()
        {
            // GET Refresh token
            var httpContext = _contextAccessor?.HttpContext;
            var refreshToken = httpContext?.Request.Cookies["refreshToken"];

            if(refreshToken is not null)
            {
                var rToken = await _unitOfWork.RefreshTokensRepository.FindAsync(rt => rt.RefreshToken == refreshToken);
                if(rToken is null || rToken.IsRevoked) { return false; }
                if(rToken is not null)
                {
                    rToken.IsRevoked = true;
                    await _unitOfWork.Complete();
                }
            }
            httpContext?.Response.Cookies.Delete("refreshToken");
            return true;
        }

        public async Task<string> ResendConfirmEmailAsync(string email)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            if (user == null) return "Something error happen please try again";
            // Prevent re-confirmation of email within 5 minutes
            if (user.LastEmailConfirmSent.HasValue &&
                DateTime.UtcNow - user.LastEmailConfirmSent.Value < TimeSpan.FromMinutes(5))
            {
                return "You can only confirm your email once every 5 minutes.";
            }
            var sendEmail = await generateTokenAndSendConfirmEmailAsync(user);
            if(!sendEmail) return "Something error happen please try again";
            return "Success";
        }

        private async Task<string> AddUserWithRoleAsync(ApplicationUser user, string password)
        {
            var transaction = await _unitOfWork.UserRepoistory.BeginTransactionAsync();
            try
            {
                // create user
                var createResult = await _unitOfWork.UserManager.CreateAsync(user, password);
                //Failed
                if (!createResult.Succeeded)
                    return string.Join(",", createResult.Errors.Select(x => x.Description).ToList());
                // add role
                await _unitOfWork.UserManager.AddToRoleAsync(user, "User");
                // commit transaction
                transaction.Commit();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        private async Task<bool> generateTokenAndSendConfirmEmailAsync(ApplicationUser user)
        {
            var transaction = await _unitOfWork.UserRepoistory.BeginTransactionAsync();
            try
            {
                var email = user.Email;
                if (email == null) return false;
                // generate confirm token
                var code = await _unitOfWork.UserManager.GenerateEmailConfirmationTokenAsync(user);

                // send email
                var request = _contextAccessor?.HttpContext?.Request;
                if (request is null) return false;
                var encodedToken = WebUtility.UrlEncode(code);
                var returnUrl = request?.Scheme + "://" + request?.Host + $"/api/v1/Auth/confirm-email?token={encodedToken}&userId={user.Id}";

                EmailMessage emailMessage = new EmailMessage()
                {
                    Body = $"To Confirm Email Click Link: <a href='{returnUrl}'>Link Of Confirmation</a>",
                    IsHtml = true,
                    To = new List<string>() { user.Email ?? "" },
                    Subject = "Confirm Email"
                };
                await _emailService.SendEmailAsync(emailMessage);
                // Prevent re-confirmation of email within 5 minutes
                user.LastEmailConfirmSent = DateTime.UtcNow;
                await _unitOfWork.UserManager.UpdateAsync(user);
                // commit transaction
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        private async Task<string> GenerateGwtToken(ApplicationUser user)
        {
            List<Claim> claims = await handleUserClaimsAsync(user);
            // security key
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
            // Create a SigningCredentials object with the security key and the HMACSHA256 algorithm.
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _tokenSettings.Issuer,
                _tokenSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_tokenSettings.AccessTokenExpireDate)),
                signingCredentials: signingCredentials
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessToken;
        }

        private ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _tokenSettings.Audience,
                ValidateIssuer = true,
                ValidIssuer = _tokenSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret)),
                ValidateLifetime = false //should be false
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<List<Claim>> handleUserClaimsAsync(ApplicationUser user)
        {
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim("UserName", user.UserName),
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email),
            };
            // roles register to claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private string? GetUserIp() => _contextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString();

        private string? GetUserAgent() => _contextAccessor?.HttpContext?.Request.Headers.UserAgent.ToString();

        private async Task<GeoLocation?> GetLocationFromIP(string ip)
        {
            // Example using an IP Geolocation API (install "IpStack.Net" or "MaxMind.GeoIP2")
            try
            {
                using var client = new HttpClient();
                var response = await client.GetStringAsync($"http://ip-api.com/json/{ip}");
                var result = JsonSerializer.Deserialize<GeoLocation>(response);
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
