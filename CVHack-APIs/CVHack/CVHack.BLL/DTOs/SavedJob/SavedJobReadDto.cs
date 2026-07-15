namespace CVHack.BLL
{
    public class SavedJobReadDto
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string JobUrl { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; }
    }
}