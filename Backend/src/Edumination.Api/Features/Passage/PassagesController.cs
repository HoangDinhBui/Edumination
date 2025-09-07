// Edumination.Api.Controllers/PassagesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Edumination.Api.Dtos;
using Edumination.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Edumination.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [Authorize]
    public class PassagesController : ControllerBase
    {
        private readonly IPassageService _passageService;
        private readonly ILogger<PassagesController> _logger;

        public PassagesController(IPassageService passageService, ILogger<PassagesController> logger)
        {
            _passageService = passageService;
            _logger = logger;
        }

        [HttpPost("sections/{sid}/passages")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> CreatePassage(long sid, [FromBody] PassageCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid: {@Errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            try
            {
                var createdPassage = await _passageService.CreatePassageAsync(sid, dto);
                return CreatedAtAction(nameof(GetPassage), new { pid = createdPassage.Id }, createdPassage);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Section not found for ID: {Sid}", sid);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Conflict error for section ID: {Sid}", sid);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error for section ID: {Sid}", sid);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("passages/{pid}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> UpdatePassage(long pid, [FromBody] PassageUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid: {@Errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            try
            {
                var updatedPassage = await _passageService.UpdatePassageAsync(pid, dto);
                return Ok(updatedPassage); // Hoặc NoContent() cho PATCH nếu không cần trả dữ liệu
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Passage not found for ID: {Pid}", pid);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Conflict error for passage ID: {Pid}", pid);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error for passage ID: {Pid}", pid);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("passages/{pid}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> DeletePassage(long pid)
        {
            try
            {
                await _passageService.DeletePassageAsync(pid);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Passage not found for ID: {Pid}", pid);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error for passage ID: {Pid}", pid);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{pid}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> GetPassage(long pid)
        {
            // Triển khai logic lấy passage (placeholder)
            return NotFound();
        }
    }
}