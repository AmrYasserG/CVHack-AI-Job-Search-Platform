namespace CVHack.DAL;

public interface IProfileSkillRepository
{
    Task<IEnumerable<ProfileSkill>> GetByProfileIdAsync(int profileId);
    Task<ProfileSkill?> GetAsync(int profileId, int skillId);
    Task<bool> ExistsAsync(int profileId, int skillId);
    void Add(ProfileSkill profileSkill);
    void Remove(ProfileSkill profileSkill);
    Task SaveChangesAsync();
}
