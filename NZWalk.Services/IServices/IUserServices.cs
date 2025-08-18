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
        public Task<UserDTO> Login(LoginDTO loginDTO, bool ApplyCache = false,CancellationToken cancellationToken = default);
        public Task<bool> CahngeRole(Guid Id,string RoleName, bool ApplyCache = false, CancellationToken cancellationToken = default);
        public Task<bool> Confirm(string token, string email,bool ApplyCache=false,CancellationToken cts=default);
        public Task<bool> reset_password(string email, bool ApplyCache = false, CancellationToken cts=default);
        public Task<bool> Confirm_pass(string token,string email,string pass, bool ApplyCache = false,CancellationToken cts=default);
    }
}
