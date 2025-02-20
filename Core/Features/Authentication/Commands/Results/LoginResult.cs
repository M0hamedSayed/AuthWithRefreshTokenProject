using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Authentication.Commands.Results
{
    public class LoginResult
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }

        public required ActiveSession activeSession { get; set; }
    }

    public class ActiveSession
    {
        // user metadata
        public string AccessToken { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string IP { get; set; } = string.Empty;

        public string? Country { get; set; }
        public string? CountryCode { get; set; }
        public string? City { get; set; }
        public string? TimeZone { get; set; }
        public decimal? LocationLat { get; set; }
        public decimal? LocationLng { get; set; }
    }
}
