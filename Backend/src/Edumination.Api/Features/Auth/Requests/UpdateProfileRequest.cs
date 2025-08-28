namespace Edumination.Api.Features.Auth.Requests
{
    public sealed class UpdateProfileRequest
    {
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
