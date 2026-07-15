using CVHack.DAL;

namespace CVHack.DAL;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<ProfileSkill> ProfileSkills { get; set; } = new List<ProfileSkill>();
}
