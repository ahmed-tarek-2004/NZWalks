using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using NZWalks.Validation;
using System.Runtime.CompilerServices;

namespace NZWalks.Controllers.V2
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionServices services;
        private readonly ICacheServices cacheServices;

        public RegionsController(ApplicationDBContext context, IRegionServices services
            , ICacheServices cacheServices)
        {
            // this.context = context;
            this.services = services;
            this.cacheServices = cacheServices;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string? Properties = null, [FromQuery] string? order = null
            , [FromQuery] bool? isDescending = false)
        {
            var regions = await services.GetALL(Properties, order, isDescending,true);

            if (regions == null)
            {
                return BadRequest();
            }
            var ListResions = regions.ToList();
            return Ok(ListResions);
        }

        [HttpGet("GetById/{Id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var regions = await services.Get(Id,true);
            if (regions == null)
            {
                return BadRequest();
            }
            return Ok(regions);
        }

        [HttpPost("Create")]
        [ServiceFilter(typeof(ValidationFilter))]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto dto)
        {

            if (dto == null)
            {
                return BadRequest();
            }
            var region = await services.Add(dto);
            return CreatedAtAction(nameof(GetById), new { region.Id }, region);
        }

        [HttpPut("Update/{id:guid}")]
        [Authorize(Roles = "Writer")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto dto)
        {
            var region = await services.Update(id, dto, true);
            if (region is null)
            {
                return BadRequest();
            }
            return Ok($"Updated \n{region}");
        }

        [HttpDelete("Delete/{Id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            var region = await services.Delete(Id, true);
            if (region is not null)
            {
                return Ok(region);

            }
            return BadRequest();
        }
    }
}