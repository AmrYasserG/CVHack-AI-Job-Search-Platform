
namespace CVHack.DAL;

public class ProfileSkill
{
    public int ProfileId { get; set; }
    public UserProfile Profile { get; set; } = null!;

    public int SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
}
