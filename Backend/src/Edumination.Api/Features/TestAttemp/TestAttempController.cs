using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Domain.Entities;
using System.Security.Claims;

namespace Edumination.Api.Features.Attempts
{
    [ApiController]
    [Route("api/v1/test-attempts")]
    public class TestAttemptController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<TestAttemptController> _logger;

        public TestAttemptController(AppDbContext db, ILogger<TestAttemptController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartTestAttempt([FromBody] StartTestAttemptRequest request)
        {
            try
            {
                // Get user from token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { error = "User not authenticated" });
                }

                // Validate user exists
                var userExists = await _db.Users.AnyAsync(u => u.Id == userId);
                if (!userExists)
                {
                    return NotFound(new { error = "User not found" });
                }

                // Validate paper exists
                var paper = await _db.TestPapers
                    .Include(p => p.TestSections)
                    .FirstOrDefaultAsync(p => p.Id == request.PaperId);

                if (paper == null)
                {
                    return NotFound(new { error = "Test paper not found" });
                }

                // Calculate attempt number
                var attemptNo = await _db.TestAttempts
                    .Where(ta => ta.UserId == userId && ta.PaperId == request.PaperId)
                    .CountAsync() + 1;


                // Check for duplicate attempt
                var existingAttempt = await _db.TestAttempts
                    .FirstOrDefaultAsync(ta => ta.UserId == userId && ta.PaperId == request.PaperId && ta.AttemptNo == attemptNo);
                if (existingAttempt != null)
                {
                    return Conflict(new { error = "Test attempt already exists for this user and paper." });
                }

                // Create TestAttempt
                var testAttempt = new TestAttempt
                {
                    UserId = userId,
                    PaperId = request.PaperId,
                    AttemptNo = attemptNo,
                    StartedAt = DateTime.UtcNow,
                    Status = "IN_PROGRESS"
                };

                _db.TestAttempts.Add(testAttempt);
                await _db.SaveChangesAsync();

                // Create SectionAttempts for each section
                var sectionAttempts = new List<SectionAttempt>();

                foreach (var section in paper.TestSections)
                {
                    var sectionAttempt = new SectionAttempt
                    {
                        TestAttemptId = testAttempt.Id,
                        SectionId = section.Id,
                        StartedAt = DateTime.UtcNow,
                        Status = "IN_PROGRESS"
                    };

                    _db.SectionAttempts.Add(sectionAttempt);
                    sectionAttempts.Add(sectionAttempt);
                }

                await _db.SaveChangesAsync();

                _logger.LogInformation("Test attempt created: TestAttemptId={TestAttemptId}, UserId={UserId}",
                    testAttempt.Id, userId);

                return Ok(new
                {
                    testAttemptId = testAttempt.Id,
                    sectionAttempts = sectionAttempts.Select(sa => new
                    {
                        id = sa.Id,
                        sectionId = sa.SectionId,
                        skill = paper.TestSections.FirstOrDefault(s => s.Id == sa.SectionId)?.Skill
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating test attempt");
                return StatusCode(500, new { error = "An error occurred while starting the test", details = ex.Message });
            }
        }

        public class StartTestAttemptRequest
        {
            public long PaperId { get; set; }
        }
    }
}