namespace CVHack.BLL
{
    public class ApplicationReadDto
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal MatchScore { get; set; }
        public bool MockInterview { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}