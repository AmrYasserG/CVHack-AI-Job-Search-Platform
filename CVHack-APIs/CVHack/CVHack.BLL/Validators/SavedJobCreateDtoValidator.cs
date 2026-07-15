using FluentValidation;

namespace CVHack.BLL
{
    public class SavedJobCreateDtoValidator : AbstractValidator<SavedJobCreateDto>
    {
        public SavedJobCreateDtoValidator()
        {
            RuleFor(x => x.JobId)
                .GreaterThan(0)
                .WithMessage("Invalid job id.");
        }
    }
}