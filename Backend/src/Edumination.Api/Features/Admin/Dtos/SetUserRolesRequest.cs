namespace Edumination.Api.Features.Admin.Dtos;

public enum RoleUpdateMode
{
    Replace = 0, // thay toàn bộ roles = danh sách gửi lên
    Add = 1,     // chỉ thêm những role chưa có
    Remove = 2   // chỉ gỡ những role có trong danh sách
}

public sealed class SetUserRolesRequest
{
    public string[] Roles { get; set; } = Array.Empty<string>(); // ["ADMIN","TEACHER"]
    public RoleUpdateMode Mode { get; set; } = RoleUpdateMode.Replace;

    // nếu Replace mà Roles rỗng, yêu cầu explicit xác nhận
    public bool AllowEmptyWhenReplace { get; set; } = false;
}