using AspNetCoreRateLimit;
using Microsoft.Extensions.DependencyInjection;
using Service.Implementations;
using Service.Interfaces;

namespace Service.Extensions
{
    public static class ModuleServicesDependencies
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // add rate limiting
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(options =>
                {
                    options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "1m",  // Limit to 30 request per 1 minutes
                        Limit = 30
                    }
                };
            });
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            // dependancy injections
            services.AddTransient<IEmailService,EmailService>();
            services.AddTransient<IAuthenticationService,AuthenticationService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IAdminService, AdminService>();

            return services;
        }
    }
}
