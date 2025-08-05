using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;

namespace NZWalks.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionServices services;

        public RegionsController(ApplicationDBContext context, IRegionServices services)
        {
            // this.context = context;
            this.services = services;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var regions = await services.GetALL();
            if (regions == null)
            {
                return BadRequest();
            }
            return Ok(regions);
        }

        [HttpGet("GetByID/{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var regions = await services.Get(Id);
            if (regions == null)
            {
                return BadRequest();
            }
            return Ok(regions);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            var region = await services.Add(dto);
            return CreatedAtAction(nameof(GetById), new { Id = region.Id },region);
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto dto)
        {
            var region = await services.Update(id, dto);
            if(region is null)
            {
                return BadRequest();
            }
            return Ok($"Updated \n{region}");
        }

        [HttpDelete("Delete/{ID:Guid}")]
        public async Task<IActionResult> Delete([FromRoute(Name = "ID")] Guid id)
        {
            var region=await services.Delete(id);
            if(region is not null)
            return Ok("Deleted");
            return BadRequest();

        }
    }
}