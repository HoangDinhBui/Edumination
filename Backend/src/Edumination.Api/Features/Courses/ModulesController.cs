// Features/Courses/ModulesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Edumination.Api.Features.Courses.Dtos;
using Edumination.Api.Features.Courses.Services;

namespace Edumination.Api.Features.Courses
{
    [ApiController]
    [Route("api/v1/modules")]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _svc;
        public ModulesController(IModuleService svc) => _svc = svc;

        // GET /api/v1/modules/{mid}/lessons
        [HttpGet("{mid:long}/lessons")]
        [AllowAnonymous] // quyền xem được check trong service
        [ProducesResponseType(typeof(List<LessonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLessons([FromRoute] long mid, CancellationToken ct)
        {
            var list = await _svc.GetLessonsAsync(mid, User, ct);
            if (list is null) return NotFound(); // module không tồn tại hoặc bị ẩn vì course chưa publish
            return Ok(list); // có thể là mảng rỗng nếu không có lesson/published nào
        }
    }
}
