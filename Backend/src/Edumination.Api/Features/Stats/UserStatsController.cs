using System.Security.Claims;
using Edumination.Api.Features.Stats.Dtos;
using Edumination.Api.Features.Stats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edumination.Api.Features.Stats.Controllers
{

    [ApiController]
    [Route("api/v1/me")]
    public class UserStatsController : ControllerBase
    {
        private readonly IUserStatsService _userStatsService;

        public UserStatsController(IUserStatsService userStatsService)
        {
            _userStatsService = userStatsService;
        }

        [HttpGet("stats")]
        [Authorize]
        public async Task<ActionResult<UserStatsDto>> GetMyStats(CancellationToken ct)
        {
            // Lấy userId từ Claims trong token
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var stats = await _userStatsService.GetUserStatsAsync(userId, ct);
            if (stats == null)
            {
                return NotFound();
            }

            return Ok(stats);
        }
    }
}