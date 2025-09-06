using FluentValidation;
using Edumination.Api.Features.Courses.Dtos;

public class CreateModuleRequestValidator : AbstractValidator<CreateModuleRequest>
{
    public CreateModuleRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255);

        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description!).MaximumLength(2000);
        });

        When(x => x.Position.HasValue, () =>
        {
            RuleFor(x => x.Position!.Value)
                .GreaterThanOrEqualTo(1).WithMessage("Position must be >= 1.");
        });
    }
}
