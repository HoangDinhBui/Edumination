namespace Edumination.Api.Features.Auth.Responses;

public sealed class MeProfileDto
{
    public long UserId { get; set; }
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string? AvatarUrl { get; set; }
    public bool EmailVerified { get; set; }
}

public sealed class MeResponse
{
    public MeProfileDto Profile { get; set; } = default!;
    public string[] Roles { get; set; } = Array.Empty<string>();
}
