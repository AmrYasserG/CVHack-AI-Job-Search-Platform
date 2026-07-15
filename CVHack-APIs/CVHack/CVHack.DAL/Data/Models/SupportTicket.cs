namespace CVHack.DAL
{
    public class SupportTicket
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string? Reply { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }
}
