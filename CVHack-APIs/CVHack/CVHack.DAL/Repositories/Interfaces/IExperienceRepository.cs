namespace CVHack.DAL;

public interface IExperienceRepository
{
    Task<List<Experience>> GetAllByUserIdAsync(string userId);
    Task<Experience?> GetByIdAsync(int id);
    Task AddAsync(Experience experience);
    void Update(Experience experience);
    void Delete(Experience experience);
    Task SaveChangesAsync();
}
