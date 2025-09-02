using Edumination.Api.Features.Admin.Dtos;
using FluentValidation;

namespace Edumination.Api.Features.Admin.Validators;

public class CreateEduDomainRequestValidator : AbstractValidator<CreateEduDomainRequest>
{
    public CreateEduDomainRequestValidator()
    {
        RuleFor(x => x.Domain)
            .NotEmpty().WithMessage("Domain is required.")
            .MaximumLength(255)
            .Matches(@"^[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")
            .WithMessage("Invalid domain format (e.g., example.edu.vn).");
    }
}
