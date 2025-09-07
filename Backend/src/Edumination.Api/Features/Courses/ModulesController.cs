// Features/Courses/ModulesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Edumination.Api.Features.Courses.Dtos;
using Edumination.Api.Features.Courses.Services;
using FluentValidation;
using Edumination.Api.Common.Results;

namespace Edumination.Api.Features.Courses
{
    [ApiController]
    [Route("api/v1/modules")]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _svc;
        private readonly IValidator<CreateLessonRequest> _createLessonValidator;
        public ModulesController(
            IModuleService svc,
            IValidator<CreateLessonRequest> createLessonValidator)
        {
            _svc = svc;
            _createLessonValidator = createLessonValidator;
        }

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

        // POST /modules/{mid}/lessons (absolute route — không gắn /api/v1/courses)
        [HttpPost("{mid:long}/lessons")]
        [Authorize] // quyền chi tiết check trong service (Owner/Teacher/Admin)
        [ProducesResponseType(typeof(ApiResult<LessonDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateLesson([FromRoute] long mid, [FromBody] CreateLessonRequest req, CancellationToken ct)
        {
            var val = await _createLessonValidator.ValidateAsync(req, ct);
            if (!val.IsValid)
            {
                var errs = val.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(new ApiResult<object>(false, null, System.Text.Json.JsonSerializer.Serialize(errs)));
            }

            var result = await _svc.CreateLessonAsync(mid, req, User, ct);

            if (!result.Success)
            {
                return result.Error switch
                {
                    string s when s.StartsWith("NOT_FOUND") => NotFound(new { error = result.Error }),
                    "FORBIDDEN" => Forbid(),
                    _ when result.Error?.StartsWith("CONFLICT") == true => Conflict(new { error = result.Error }),
                    _ => BadRequest(result)
                };
            }

            // Trả 201; Location trỏ về list lessons của module
            return CreatedAtAction(nameof(GetLessons), new { mid }, result);
        }
    }
}
