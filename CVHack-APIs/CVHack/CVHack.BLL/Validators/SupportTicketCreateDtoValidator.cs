using FluentValidation;

namespace CVHack.BLL
{
    public class SupportTicketCreateDtoValidator : AbstractValidator<SupportTicketCreateDto>
    {
        public SupportTicketCreateDtoValidator()
        {
            RuleFor(x => x.Subject)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Subject is required.")
                .MaximumLength(200)
                .WithMessage("Subject must not exceed 200 characters.");

            RuleFor(x => x.Category)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Category is required.")
                .MaximumLength(100)
                .WithMessage("Category must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(2000)
                .WithMessage("Description must not exceed 2000 characters.");
        }
    }
}
