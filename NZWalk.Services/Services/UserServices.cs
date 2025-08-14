using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using NZWalk.utility;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace NZWalk.Services.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<UserServices> logger;
        private readonly ICacheServices cacheServices;
        private readonly IEmailSender emailSender;
        public UserServices(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager
            , IEmailSender emailSender,ICacheServices cacheServices,ILogger<UserServices>logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
            this.cacheServices = cacheServices;
            this.logger = logger;
        }

        public async Task<bool> CahngeRole(Guid Id, string RoleName, bool ApplyCache = false, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(RoleName) || string.IsNullOrEmpty(Id.ToString()))
            {
                return false;
            }
            var user = new IdentityUser();
            if(ApplyCache)
            {
                user = await cacheServices.GetCache<IdentityUser>($"User-{user.Id}");
                if (user == null)
                {
                    logger.LogInformation("Cache User");
                    user = await userManager.FindByIdAsync(Id.ToString());
                }
            }
            else
               user = await userManager.FindByIdAsync(Id.ToString());
            if (user != null)
            {
                var userRole = await userManager.GetRolesAsync(user);
                if (userRole == null || !userRole.Any())
                {
                    return false;
                }
                if (userRole.Contains(RoleName))
                    return true;
                await userManager.RemoveFromRoleAsync(user, userRole.FirstOrDefault()!);
                await userManager.AddToRoleAsync(user, RoleName);
                return true;
            }
            return false;
        }

        public async Task<UserDTO> Login(LoginDTO loginDTO, bool ApplyCache=false,CancellationToken cancellationToken = default)
        {
            var user = new IdentityUser();
            if(ApplyCache)
            {
                user = await cacheServices.GetCache<IdentityUser>($"User-{user.Id}");
                logger.LogInformation("Cache User");
                if (user== null)
                {
                    user = await userManager.FindByEmailAsync(loginDTO.Email);
                    if(user!=null)
                    await cacheServices.SetCache<IdentityUser>($"User-{user.Id}",user);
                }
            }
            else
            user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return null;
            }
            var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (result == false)
            {
                return null;
            }
            var UserRoles = (List<string>)await userManager.GetRolesAsync(user);
            return new UserDTO()
            {
                roles = UserRoles,
                user = user
            };

        }

        public async Task<IdentityResult> Register(RegisterDTO registerDTO, CancellationToken cancellationToken = default)
        {
            var user = new IdentityUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email.Substring(0, registerDTO.Email.IndexOf("@")),
                //PasswordHash = registerDTO.Password
            };
            //await userManager.ConfirmEmailAsync(user, "Ahmed@gmail.com");
            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (registerDTO.Roles == null || !registerDTO.Roles.Any() || !result.Succeeded)
            {
                return result;
            }
            else
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var confirmLink = $"https://ahmed-tarek2023.github.io/EmailConfirmation/?token={code}&email={user.Email}";

                await emailSender.SendEmailAsync(user.Email, "Confirm Your Email", $@"
                                                <h2>Confirm Your Email</h2>
                                                <p>Hello {user.Email.Substring(0, user.Email.IndexOf("@"))},</p>
                                                <p>Please click the link below to confirm your email:</p>
                                                <p><a href='{confirmLink}'>Confirm Email</a></p>
                                                <p>If you did not register, ignore this email.</p>");

                return await userManager.AddToRolesAsync(user, registerDTO.Roles);
            }
        }
        public async Task<bool> Confirm(string token, string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return false;
            }
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            var result = await userManager.ConfirmEmailAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token)));
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
