namespace CVHack.DAL;

public interface ISkillRepository : IGenericRepository<Skill>
{
    Task<IEnumerable<Skill>> SearchAsync(string? search);
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
}
