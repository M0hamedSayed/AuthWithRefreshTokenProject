using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers
{
    public class GeoLocation
    {
        public string? country {  get; set; }
        public string? countryCode { get; set; }
        public string? city { get; set; }
        public string? timezone { get; set; }
        public decimal? lat { get; set; }
        public decimal? lon { get; set; }

    }
}
