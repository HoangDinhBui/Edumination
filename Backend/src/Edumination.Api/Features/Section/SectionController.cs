using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Edumination.Api.Dtos;
using Edumination.Api.Services.Interfaces;

namespace Edumination.Api.Controllers
{
    [ApiController]
    [Route("api/sections")]
    [Authorize] // Require authentication
    public class SectionsController : ControllerBase
    {
        private readonly ISectionService _sectionService;

        public SectionsController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [HttpPatch("{sid}")]
        [Authorize(Roles = "TEACHER,ADMIN")] // Chỉ TEACHER hoặc ADMIN
        public async Task<IActionResult> UpdateSection(long sid, [FromBody] UpdateSectionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedSection = await _sectionService.UpdateSectionAsync(sid, dto);
                return Ok(updatedSection); // Hoặc NoContent() cho PATCH
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                // Log error (sử dụng IAuditLogger nếu đã có)
                return StatusCode(500, "Internal server error");
            }
        }
    }
}