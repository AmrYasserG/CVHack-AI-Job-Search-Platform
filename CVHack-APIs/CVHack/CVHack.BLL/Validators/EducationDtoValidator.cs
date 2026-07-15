using FluentValidation;

namespace CVHack.BLL;

public class EducationDtoValidator : AbstractValidator<EducationDto>
{
    public EducationDtoValidator()
    {
        RuleFor(x => x.University)
            .NotEmpty().WithMessage("University is required.")
            .MaximumLength(200).WithMessage("University must not exceed 200 characters.");

        RuleFor(x => x.Degree)
            .MaximumLength(150).WithMessage("Degree must not exceed 150 characters.")
            .When(x => !string.IsNullOrEmpty(x.Degree));

        RuleFor(x => x.Grade)
            .MaximumLength(50).WithMessage("Grade must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Grade));

        RuleFor(x => x.StartYear)
            .InclusiveBetween(1950, DateTime.UtcNow.Year).WithMessage($"Start Year must be between 1950 and {DateTime.UtcNow.Year}.")
            .When(x => x.StartYear.HasValue);

        RuleFor(x => x.EndYear)
            .GreaterThanOrEqualTo(x => x.StartYear!.Value).WithMessage("End Year must be greater than or equal to Start Year.")
            .When(x => x.EndYear.HasValue && x.StartYear.HasValue);
    }
}
