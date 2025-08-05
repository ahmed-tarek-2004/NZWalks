using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public RegionsController(ApplicationDBContext context)
        {
            this.context = context;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var region = context.Regions.ToList();
            if (region != null)
            {

                return Ok(region);
            }
            return BadRequest();

        }
        [HttpGet("Get_By_ID {Id:Guid}")]
        public IActionResult GetById([FromRoute] Guid Id)
        {
            var region = context.Regions.Find(Id);
            if (region == null)
            {

                return Ok(region);
            }
            return BadRequest();

        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] AddRegionRequestDto dto)
        {
            var region = new Region()
            {
                Name = dto.Name,
                Code = dto.Code,
                RegionImageUrl = dto.RegionImageUrl
            };
            context.Regions.Add(region);
            context.SaveChanges();
            var regoinDTO = new RegionDTO()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { Id = region.Id }, regoinDTO);
        }

        [HttpPut("Update/{id:guid}")]
        public IActionResult Update([FromRoute]Guid id, [FromBody] UpdateRegionRequestDto dto)
        {
            var region = context.Regions.Find(id);
            if (region == null||dto is null)
            {
                return BadRequest();
            }
            region.Name = dto.Name;
            if(dto.RegionImageUrl != null) 
            region.RegionImageUrl = dto.RegionImageUrl;
            region.Code = dto.Code;
            context.SaveChanges();

            return Ok("Updated");
        }

        [HttpDelete("Delete/{id:Guid}")]
        public IActionResult Delete([FromRoute]Guid id)
        {
            var region = context.Regions.FirstOrDefault(r=>r.Id==id);
            if(region == null)
            {
                return BadRequest();
            }
            context.Remove(region);
            context.SaveChanges();
            return Ok("Deleted");
        }
    }
}
