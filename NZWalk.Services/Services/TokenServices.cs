using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NZWalk.DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.Services.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration configuration;

        public TokenServices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public async Task<JwtSecurityToken> CreateJWT(IdentityUser user,List<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };

            claims.AddRange(roles.Select(r=>new Claim(ClaimTypes.Role,r)));
            var SignInKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                                   configuration["JWT:SecurityKey"]));


            var signIn = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);
            return new JwtSecurityToken(
                issuer: configuration["JWT:IssuerIP"],
                audience: configuration["JWT:AudienceIP"],
                claims: claims,
                signingCredentials: signIn,
                expires: DateTime.Now.AddMinutes(14)
                );
        }
        /*
           "JWT": {
    "SecurityKey": "asdadaqeq!@##3244343dsdfsfvdiui98ghj55irurtbtrb549owi%^&$#",
    "AudienceIP": "http://localhost:4200/",
    "IssuerIP": "http://localhost:5146/"
  }
         */
    }
}
