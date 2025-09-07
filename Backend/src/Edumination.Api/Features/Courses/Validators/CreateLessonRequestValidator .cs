using FluentValidation;
using Edumination.Api.Features.Courses.Dtos;

public class CreateLessonRequestValidator : AbstractValidator<CreateLessonRequest>
{
    public CreateLessonRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255);

        When(x => x.Objective != null, () =>
        {
            RuleFor(x => x.Objective!).MaximumLength(4000);
        });

        When(x => x.Position.HasValue, () =>
        {
            RuleFor(x => x.Position!.Value)
                .GreaterThanOrEqualTo(1).WithMessage("Position must be >= 1.");
        });
    }
}
