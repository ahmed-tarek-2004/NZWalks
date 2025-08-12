using Asp.Versioning;
using Microsoft.AspNetCore.Http;
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
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await userServices.Login(loginDTO);
            if (result != null)
            {
                var Token = await tokenServices.CreateJWT(result.user, result.roles);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(Token),
                    Expires = DateTime.UtcNow.AddMinutes(15)
                });
               
            }
            return BadRequest("Error on login");
        }
    }

}
