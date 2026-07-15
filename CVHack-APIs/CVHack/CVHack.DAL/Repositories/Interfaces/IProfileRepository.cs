namespace CVHack.DAL;

public interface IProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(string userId);
    Task<bool> ExistsForUserAsync(string userId);
    Task SaveChangesAsync();
    Task<UserProfile?> GetFullProfileAsync(string userId);
}
