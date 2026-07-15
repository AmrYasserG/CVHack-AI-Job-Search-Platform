namespace CVHack.DAL;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<IEnumerable<Project>> GetByProfileIdAsync(int profileId);
    Task<Project?> GetByIdAndProfileAsync(int id, int profileId);
}
