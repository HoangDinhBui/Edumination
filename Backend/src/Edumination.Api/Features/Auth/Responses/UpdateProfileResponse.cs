namespace Edumination.Api.Features.Auth.Responses
{
    public sealed class UpdateProfileResponse
    {
        public long UserId { get; set; }
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string? AvatarUrl { get; set; }
        public bool EmailVerified { get; set; }
    }
}