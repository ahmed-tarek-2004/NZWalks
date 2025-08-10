using Microsoft.AspNetCore.Identity;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.Services.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<IdentityUser> userManager;
        public UserServices(UserManager<IdentityUser>userManager) 
        {
            this.userManager= userManager;
        }

        public async Task<UserDTO> Login(LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return null;
            }
            var result = await userManager.CheckPasswordAsync(user,loginDTO.Password);
            if(result==false)
            {
                return null;
            }
            var UserRoles = (List<string>)await userManager.GetRolesAsync(user);
            return new UserDTO()
            {
                roles =UserRoles,
                user=user
            };

        }

        public async Task<IdentityResult> Register(RegisterDTO registerDTO)
        {
            var user = new IdentityUser()
            {
                Email = registerDTO.Email,
                UserName=registerDTO.Email.Substring(0,registerDTO.Email.IndexOf("@")),
                //PasswordHash = registerDTO.Password
            };
            var result= await userManager.CreateAsync(user,registerDTO.Password);
            if (registerDTO.Roles == null||!registerDTO.Roles.Any()||!result.Succeeded)
            {
                return result;
            }
            else 
            {
                return await userManager.AddToRolesAsync(user, registerDTO.Roles);
            }


        }
    }
}
