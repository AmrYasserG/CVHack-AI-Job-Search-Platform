namespace CVHack.BLL
{
    public class UpdateUserStatusDto
    {
        public string Status { get; set; } = string.Empty;   // "Active" | "Suspended"
    }

    public class UpdateUserPlanDto
    {
        public string Plan { get; set; } = string.Empty;      // "Free" | "Pro"
    }
}