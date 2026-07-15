namespace CVHack.DAL;

public interface ICertificationRepository
{
    Task<List<Certification>> GetAllByUserIdAsync(string userId);
    Task<Certification?> GetByIdAsync(int id);
    Task AddAsync(Certification certification);
    void Update(Certification certification);
    void Delete(Certification certification);
    Task SaveChangesAsync();
}
