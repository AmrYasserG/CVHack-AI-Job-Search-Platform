namespace CVHack.DAL;

public class Certification
{
    public int Id { get; set; }

    public int ProfileId { get; set; }
    public UserProfile Profile { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string? Provider { get; set; }
    public string? CredentialUrl { get; set; }
    public DateTime? CertifiedAt { get; set; }
}
