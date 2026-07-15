using FluentValidation;

namespace CVHack.BLL
{
    public class ApplicationCreateDtoValidator : AbstractValidator<ApplicationCreateDto>
    {
        public ApplicationCreateDtoValidator()
        {
            RuleFor(x => x.JobId)
                .GreaterThan(0)
                .WithMessage("Invalid job id.");
        }
    }
}