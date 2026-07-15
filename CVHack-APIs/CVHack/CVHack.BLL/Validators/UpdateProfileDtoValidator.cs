using FluentValidation;

namespace CVHack.BLL;

public class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
{
    public UpdateProfileDtoValidator()
    {
        RuleFor(x => x.Headline)
            .MaximumLength(220).WithMessage("Headline must not exceed 220 characters.");

        RuleFor(x => x.Summary)
            .MaximumLength(2600).WithMessage("Summary must not exceed 2600 characters.");

        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters.");

        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone Number must not exceed 20 characters.")
            .Matches(@"^[\d\+\-\s\(\)]*$").WithMessage("Phone Number format is invalid.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.LinkedInUrl)
            .MaximumLength(500).WithMessage("LinkedIn URL must not exceed 500 characters.")
            .Must(BeAValidUrl).WithMessage("LinkedIn URL must be a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.LinkedInUrl));

        RuleFor(x => x.GitHubUrl)
            .MaximumLength(500).WithMessage("GitHub URL must not exceed 500 characters.")
            .Must(BeAValidUrl).WithMessage("GitHub URL must be a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.GitHubUrl));

        RuleFor(x => x.PortfolioUrl)
            .MaximumLength(500).WithMessage("Portfolio URL must not exceed 500 characters.")
            .Must(BeAValidUrl).WithMessage("Portfolio URL must be a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.PortfolioUrl));

        RuleFor(x => x.JobTitle)
            .MaximumLength(150).WithMessage("Job Title must not exceed 150 characters.");
    }

    private static bool BeAValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var result)
            && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}
