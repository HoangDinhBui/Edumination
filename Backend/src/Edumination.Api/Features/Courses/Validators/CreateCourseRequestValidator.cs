using FluentValidation;
using Edumination.Api.Features.Courses.Dtos;

namespace Edumination.Api.Features.Courses.Validators;

public class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
{
    private static readonly string[] Allowed =
    {
        "BEGINNER","ELEMENTARY","PRE_INTERMEDIATE","INTERMEDIATE","UPPER_INTERMEDIATE","ADVANCED"
    };

    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Level)
            .NotEmpty()
            .Must(l => Allowed.Contains(l.Trim().ToUpper()))
            .WithMessage("level must be one of: " + string.Join(", ", Allowed));
    }
}
