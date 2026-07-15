namespace CVHack.BLL;

public class ProfileResponseDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = default!;

    public string? Headline { get; set; }
    public string? Summary { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? PortfolioUrl { get; set; }
    public string? JobTitle { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<ExperienceResponseDto> Experiences { get; set; } = new();
    public List<EducationResponseDto> Educations { get; set; } = new();
    public List<CertificationResponseDto> Certifications { get; set; } = new();
}
