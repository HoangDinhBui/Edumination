using Edumination.Api.Features.Auth.Requests;
using FluentValidation;

public sealed class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(255);

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl));
    }
}
