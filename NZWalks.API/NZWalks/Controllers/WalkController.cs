using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using NZWalks.Validation;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkController : ControllerBase
    {

        private readonly IWalkServices services;

        public WalkController(ApplicationDBContext context, IWalkServices services)
        {
            // this.context = context;
            this.services = services;
        }

        [Authorize(Roles ="Reader,Writer")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] string? Properity, [FromQuery]string? order, [FromQuery]bool?isDescending=false,
            [FromQuery]int PageNum =1 ,[FromQuery] int PageSize=1000)
        {
            var Walks = await services.GetALL(Properity,order,isDescending,PageNum,PageSize);
            if (Walks == null)
            {
                return BadRequest();
            }
            return Ok(Walks);
        }

        [Authorize(Roles = "Reader,Writer")]
        [HttpGet("GetById/{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var Walks = await services.Get(Id);
            if (Walks == null)
            {
                return BadRequest();
            }
            return Ok(Walks);
        }

        [Authorize(Roles = "Writer")]
        [HttpPost("Create")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<IActionResult> Create([FromBody] AddWalkDto dto)
        {
           
                if (dto == null)
                {
                    return BadRequest();
                }
                var Walk = await services.Add(dto);
                return CreatedAtAction(nameof(GetById), new { Id = Walk.Id }, Walk);
       
        }
        [Authorize(Roles = "Writer")]
        [HttpPut("Update/{id:guid}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDto dto)
        {
           
                var Walk = await services.Update(id, dto);
                if (Walk == null)
                {
                    return BadRequest();
                }
                return Ok($"Updated \n{Walk}");
        }

        [Authorize(Roles = "Writer")]
        [HttpDelete("Delete/{ID:Guid}")]
        public async Task<IActionResult> Delete([FromRoute(Name = "ID")] Guid id)
        {
            var Walk = await services.Delete(id);
            if (Walk is not null)
                return Ok("Deleted");
            return BadRequest();
        }
    }
}
