using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Core.Extensions
{
    public static class ErrorHandler
    {
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder app)
        { 
            return app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
