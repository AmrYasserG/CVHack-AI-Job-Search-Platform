

namespace CVHack.DAL;

public interface IEducationRepository
{
    Task<List<Education>> GetAllByUserIdAsync(string userId);
    Task<Education?> GetByIdAsync(int id);
    Task AddAsync(Education education);
    void Update(Education education);
    void Delete(Education education);
    Task SaveChangesAsync();
}

