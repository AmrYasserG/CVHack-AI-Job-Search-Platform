namespace CVHack.BLL;

public class CertificationDto
{
    public string Name { get; set; } = default!;
    public string? Provider { get; set; }
    public string? CredentialUrl { get; set; }
    public DateTime? CertifiedAt { get; set; }
}
