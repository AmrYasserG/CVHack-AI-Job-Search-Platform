namespace CVHack.BLL
{
    public class SupportTicketListDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
