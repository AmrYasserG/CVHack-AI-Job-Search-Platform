using FluentValidation;

namespace CVHack.BLL;

public class ExperienceDtoValidator : AbstractValidator<ExperienceDto>
{
    public ExperienceDtoValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company Name is required.")
            .MaximumLength(200).WithMessage("Company Name must not exceed 200 characters.");

        RuleFor(x => x.JobTitle)
            .NotEmpty().WithMessage("Job Title is required.")
            .MaximumLength(150).WithMessage("Job Title must not exceed 150 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start Date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Start Date cannot be in the future.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End Date must be on or after Start Date.")
            .When(x => x.EndDate.HasValue);
    }
}
