using System.Security.Claims;
using Edumination.Api.Common.Results;
using Edumination.Api.Features.Courses.Dtos;

namespace Edumination.Api.Features.Courses.Services;

public interface ICourseService
{
    Task<PagedResult<CourseItemDto>> GetAsync(CourseListQuery query, ClaimsPrincipal? user, CancellationToken ct);
    Task<bool> EnrollAsync(long courseId, long userId, CancellationToken ct);
    Task<bool> UnenrollAsync(long courseId, long userId, CancellationToken ct);
    Task<CourseDetailDto?> GetDetailAsync(long id, ClaimsPrincipal? user, CancellationToken ct);
    Task<ApiResult<CreateCourseResponse>> CreateAsync(CreateCourseRequest req, long creatorUserId, CancellationToken ct);
    Task<ApiResult<CourseDetailDto>> UpdatePartialAsync(long id, UpdateCourseRequest req, ClaimsPrincipal user, CancellationToken ct);
    Task<List<ModuleDto>?> GetModulesAsync(long courseId, ClaimsPrincipal? user, CancellationToken ct);
    Task<ApiResult<ModuleDto>> CreateModuleAsync(
        long courseId, CreateModuleRequest req, ClaimsPrincipal user, CancellationToken ct);
    Task<ApiResult<object>> DeleteAsync(long id, ClaimsPrincipal user, CancellationToken ct);
}