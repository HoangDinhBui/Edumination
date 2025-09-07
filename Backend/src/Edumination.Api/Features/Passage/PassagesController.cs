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
    [Route("api/sections/{sid}/passages")]
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

        [HttpPost]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> CreatePassage(long sid, [FromBody] PassageCreateDto dto)
        {
            _logger.LogInformation("Received request for section ID: {Sid}", sid);
            _logger.LogInformation("Request Headers: {@Headers}", HttpContext.Request.Headers);
            _logger.LogInformation("Request Body: {@Body}", dto);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid: {@Errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            try
            {
                var createdPassage = await _passageService.CreatePassageAsync(sid, dto);
                return CreatedAtAction(nameof(GetPassage), new { sid, pid = createdPassage.Id }, createdPassage);
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

        [HttpGet("{pid}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> GetPassage(long sid, long pid)
        {
            return NotFound(); // Placeholder
        }
    }
}