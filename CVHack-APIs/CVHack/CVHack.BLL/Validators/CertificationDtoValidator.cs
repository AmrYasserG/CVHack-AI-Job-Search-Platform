using FluentValidation;

namespace CVHack.BLL;

public class CertificationDtoValidator : AbstractValidator<CertificationDto>
{
    public CertificationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Certification Name is required.")
            .MaximumLength(200).WithMessage("Certification Name must not exceed 200 characters.");

        RuleFor(x => x.Provider)
            .MaximumLength(200).WithMessage("Provider must not exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.Provider));

        RuleFor(x => x.CredentialUrl)
            .MaximumLength(500).WithMessage("Credential URL must not exceed 500 characters.")
            .Must(BeAValidUrl).WithMessage("Credential URL must be a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.CredentialUrl));

        RuleFor(x => x.CertifiedAt)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Certified date cannot be in the future.")
            .When(x => x.CertifiedAt.HasValue);
    }

    private static bool BeAValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var result)
            && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}
