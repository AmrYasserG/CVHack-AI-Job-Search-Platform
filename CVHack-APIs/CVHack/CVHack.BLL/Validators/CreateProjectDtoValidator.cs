using FluentValidation;

namespace CVHack.BLL;

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Project title is required.")
            .MaximumLength(150).WithMessage("Project title must not exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(3000).WithMessage("Description must not exceed 3000 characters.")
            .When(x => x.Description != null);

        RuleFor(x => x.GithubUrl)
            .MaximumLength(500).WithMessage("GitHub URL must not exceed 500 characters.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var result)
                         && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
            .WithMessage("GitHub URL must be a valid HTTP/HTTPS URL.")
            .When(x => !string.IsNullOrEmpty(x.GithubUrl));
    }
}
