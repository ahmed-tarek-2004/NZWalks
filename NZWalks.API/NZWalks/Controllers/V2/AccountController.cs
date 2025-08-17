using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using System.IdentityModel.Tokens.Jwt;

namespace NZWalks.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserServices userServices;
        private readonly ITokenServices tokenServices;
        public AccountController(IUserServices userServices,ITokenServices tokenServices)
        {
            this.userServices = userServices;
            this.tokenServices = tokenServices;

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await userServices.Register(registerDTO);
            if (result.Succeeded == true)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }
        [EnableRateLimiting("SlidingWindow")]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await userServices.Login(loginDTO,true);
            if (result != null)
            {
                var Token = await tokenServices.CreateJWT(result.user, result.roles);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(Token),
                    Expires = DateTime.UtcNow.AddMinutes(15)
                });
               
            }
            return Unauthorized("User Not Found");
        }
        [Authorize(Roles = "Writer")]
        [HttpGet("ChangeRole/{Id:guid}")]
        public async Task<IActionResult> ChangeRole([FromRoute] Guid Id, [FromQuery] string RoleName)
        {
            var result = await userServices.CahngeRole(Id, RoleName,true);
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
    }
}
