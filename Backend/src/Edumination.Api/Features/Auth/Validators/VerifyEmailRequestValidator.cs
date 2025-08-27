using FluentValidation;

namespace Edumination.Api.Features.Auth.Requests;

public class VerifyEmailRequestValidator : AbstractValidator<VerifyEmailRequest>
{
    public VerifyEmailRequestValidator()
    {
        RuleFor(x => x.Token).NotEmpty().MaximumLength(512);
    }
}