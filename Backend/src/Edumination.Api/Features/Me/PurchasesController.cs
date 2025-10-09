using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Edumination.Api.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;

namespace Edumination.Api.Features.Me;

[ApiController]
[Route("api/v1/me")]
public class PurchasesController(AppDbContext db) : ControllerBase
{
    private long? GetUserId() =>
        long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                      User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;

    [HttpGet("purchases")]
    [Authorize]
    public async Task<IActionResult> GetPurchases(CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var data = await db.Enrollments
            .Where(e => e.UserId == userId)
            .Join(db.Courses, e => e.CourseId, c => c.Id, (e, c) => new
            {
                courseId = c.Id,
                c.Title,
                purchasedAt = e.EnrolledAt
            })
            .OrderByDescending(x => x.purchasedAt)
            .ToListAsync(ct);

        return Ok(data);
    }
}
