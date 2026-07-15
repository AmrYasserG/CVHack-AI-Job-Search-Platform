using FluentValidation;

namespace CVHack.BLL
{
    public class UpdateTicketStatusDtoValidator : AbstractValidator<UpdateTicketStatusDto>
    {
        public UpdateTicketStatusDtoValidator()
        {
            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Status is required.")
                .Must(status => new[]
                {
                "Open",
                "In Progress",
                "Closed"
                }.Contains(status))
                .WithMessage("Status must be Open, In Progress or Closed.");

            RuleFor(x => x.Reply)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Reply is required.")
                .MaximumLength(2000)
                .WithMessage("Reply must not exceed 2000 characters.");
        }
    }
}
