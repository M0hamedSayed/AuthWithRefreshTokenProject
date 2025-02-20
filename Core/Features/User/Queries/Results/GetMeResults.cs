using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.User.Queries.Results
{
    public class GetMeResults
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public List<ActiveUserSession?>? activeSessions { get; set; }
    }

    public class ActiveUserSession
    {
        // user metadata
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
