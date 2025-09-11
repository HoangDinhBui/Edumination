using System.Security.Claims;
using Edumination.Api.Common.Results;
using Edumination.Api.Features.Courses.Dtos;

namespace Edumination.Api.Features.Courses.Services;

public interface IModuleService
{
    Task<List<LessonDto>?> GetLessonsAsync(long moduleId, ClaimsPrincipal? user, CancellationToken ct);
    Task<ApiResult<LessonDto>> CreateLessonAsync(
        long moduleId, CreateLessonRequest req, ClaimsPrincipal user, CancellationToken ct);
}