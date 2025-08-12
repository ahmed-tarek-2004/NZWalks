using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.utility
{
    public class JWTToken
    {
        public string SecurityKey { get; set; }
        public string AudienceIP { get; set; }
        public string IssuerIP { get; set; }
    }
}
