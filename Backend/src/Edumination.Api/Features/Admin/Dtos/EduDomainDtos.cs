namespace Edumination.Api.Features.Admin.Dtos;

public sealed class EduDomainListQuery
{
    public string? q { get; set; }        // tìm kiếm theo domain chứa chuỗi
    public int page { get; set; } = 1;    // trang bắt đầu từ 1
    public int pageSize { get; set; } = 20; // 5–100 tùy ý
}

public sealed class EduDomainItemDto
{
    public long Id { get; set; }
    public string Domain { get; set; } = default!;
}

public sealed class CreateEduDomainRequest
{
    public string Domain { get; set; } = default!;
}