using Edumination.Api.Common.Results;
using Edumination.Api.Features.Admin.Dtos;

namespace Edumination.Api.Features.Admin.Services;

public interface IAdminUsersService
{
    Task<PagedResult<AdminUserItemDto>> GetUsersAsync(AdminUserQuery q, CancellationToken ct);
    Task<ApiResult<CreateUserResponse>> CreateUserAsync(CreateUserRequest req, CancellationToken ct);
    Task<ApiResult<AdminUserDto>> PatchUserAsync(long id, PatchUserRequest req, CancellationToken ct);
    Task<ApiResult<AdminUserDto>> SetRolesAsync(long userId, SetUserRolesRequest req, CancellationToken ct);
    Task<ApiResult<AdminUserDto>> RemoveRoleAsync(long userId, string roleCode, CancellationToken ct);

}
