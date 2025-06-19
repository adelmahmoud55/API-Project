using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction.Models.Auth
{
    public class JwtSettings
    {
        public required string Key { get; set; }
        public required string Audiance { get; set; }
               
        public required string Issuer { get; set; }
               
        public required double DurationInMinutes { get; set; }
    }
}
