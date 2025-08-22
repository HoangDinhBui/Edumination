using Microsoft.AspNetCore.Mvc;
using Edumination.Api.Features.Leaderboard.Interfaces;

namespace Edumination.Api.Features.Leaderboard.Controllers;

[ApiController]
[Route("api/v1/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _service;
    public LeaderboardController(ILeaderboardService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] long paperId, [FromQuery] int limit = 100, CancellationToken ct = default)
    {
        var data = await _service.GetLeaderboardAsync(paperId, limit, ct);
        return Ok(data);
    }
}
