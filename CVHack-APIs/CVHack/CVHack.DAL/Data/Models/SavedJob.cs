namespace CVHack.DAL
{
    public class SavedJob
    {
        public string UserId { get; set; } = null!;
        public int JobId { get; set; }
        public DateTime SavedAt { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Job Job { get; set; } = null!;
    }
}
