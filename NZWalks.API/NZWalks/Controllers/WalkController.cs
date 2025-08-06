using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;

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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var Walks = await services.GetALL();
            if (Walks == null)
            {
                return BadRequest();
            }
            return Ok(Walks);
        }

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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AddWalkDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            var Walk = await services.Add(dto);
            return CreatedAtAction(nameof(GetById), new { Id = Walk.Id }, Walk);
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDto dto)
        {
            var Walk = await services.Update(id, dto);
            if (Walk == null)
            {
                return BadRequest();
            }
            return Ok($"Updated \n{Walk}");
        }

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
