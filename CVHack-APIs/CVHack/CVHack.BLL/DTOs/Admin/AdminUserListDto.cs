namespace CVHack.BLL
{
    public class AdminUserListDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
        public string Plan { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Searches { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}