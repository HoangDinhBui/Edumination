using FluentValidation;
using Edumination.Api.Features.Courses.Dtos;

namespace Edumination.Api.Features.Courses.Validators;

public class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseRequest>
{
    public UpdateCourseRequestValidator()
    {
        When(x => x.Title != null, () =>
        {
            RuleFor(x => x.Title!)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(255);
        });

        When(x => x.Level != null, () =>
        {
            RuleFor(x => x.Level!)
                .Must(lvl =>
                {
                    var ok = Enum.TryParse<Domain.Entities.CourseLevel>(lvl.Trim(), true, out _);
                    return ok;
                })
                .WithMessage("Invalid level. Allowed: BEGINNER, ELEMENTARY, PRE_INTERMEDIATE, INTERMEDIATE, UPPER_INTERMEDIATE, ADVANCED.");
        });
    }
}
