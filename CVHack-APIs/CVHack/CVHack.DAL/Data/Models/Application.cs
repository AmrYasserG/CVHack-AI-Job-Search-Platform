namespace CVHack.DAL
{
    public class Application
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int JobId { get; set; }
        public bool MockInterview { get; set; }
        public decimal MatchScore { get; set; }
        public DateTime AppliedAt { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Job Job { get; set; } = null!;
    }
}