namespace CVHack.BLL;

public class CertificationResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Provider { get; set; }
    public string? CredentialUrl { get; set; }
    public DateTime? CertifiedAt { get; set; }
}
