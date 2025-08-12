using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.IRepository
{
    public interface ITokenServices
    {
        public Task<JwtSecurityToken> CreateJWT(IdentityUser user, List<string> roles, CancellationToken cancellationToken = default);
    }
}
