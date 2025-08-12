using Microsoft.AspNetCore.Identity;
using NZWalk.DataAccess.Model.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.Services.IServices
{
    public interface IUserServices
    {

        public Task<IdentityResult> Register(RegisterDTO registerDTO, CancellationToken cancellationToken = default);
        public Task<UserDTO> Login(LoginDTO loginDTO, CancellationToken cancellationToken = default);
      
    }
}
