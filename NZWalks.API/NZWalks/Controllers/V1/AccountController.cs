using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using System.IdentityModel.Tokens.Jwt;

namespace NZWalks.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserServices userServices;
        private readonly ITokenServices tokenServices;

        public AccountController(IUserServices userServices, ITokenServices tokenServices)
        {
            this.userServices = userServices;
            this.tokenServices = tokenServices;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await userServices.Register(registerDTO);
            if (result.Succeeded)
            {
                return Ok("Check Your Inbox For Email Confirmation");
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await userServices.Login(loginDTO);
            if (result != null)
            {
                if (!result.user.EmailConfirmed) return Unauthorized("Confirm Your Email");
                var Token = await tokenServices.CreateJWT(result.user, result.roles);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(Token),
                    Expires = DateTime.UtcNow.AddMinutes(15)
                });

            }
            return Unauthorized("Not Found");
        }

        [Authorize(Roles = "Writer")]
        [HttpGet("ChangeRole/{Id:guid}")]
        public async Task<IActionResult> ChangeRole([FromRoute] Guid Id, [FromQuery] string RoleName)
        {
            var result = await userServices.CahngeRole(Id, RoleName);
            if (result)
                return Ok();
            else
                return NotFound();
        }
        [HttpGet("confirm")]
        public async Task<IActionResult> confirm([FromQuery] string token, [FromQuery] string email)
        {
            var result = await userServices.Confirm(token, email);
            if (result)
                return Ok();
            else return BadRequest();
        }
        [HttpGet("reset-password")]
        public async Task<IActionResult> reset_password([FromQuery] string email)
        {
            var result = await userServices.reset_password(email);
            if (result)
                return Ok("Check Your Email To Complete Reset Process");
            else return BadRequest();
        }
        [HttpPost("confirm-pass")]
        public async Task<IActionResult> confirm_pass([FromBody] ResetPassword reset)
        {
            if (ModelState.IsValid)
            {
                var result = await userServices.Confirm_pass( reset.Token, reset.Email, reset.Password);
                if (result)
                    return Ok("Reset Successful");
                else return BadRequest();
            }
            return BadRequest(ModelState);
        }
    }
}
