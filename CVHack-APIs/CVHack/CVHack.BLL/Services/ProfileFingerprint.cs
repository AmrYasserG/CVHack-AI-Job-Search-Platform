using System.Security.Cryptography;
using System.Text;
using CVHack.DAL;

namespace CVHack.BLL;

// Single source of truth for "has the profile meaningfully changed".
// Used by both SkillAnalysisService and MatchScoringService to invalidate caches.
public static class ProfileFingerprint
{
    public static string Compute(UserProfile p)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(BuildSignature(p)));
        return Convert.ToHexString(bytes);
    }

    // Deterministic text snapshot of the profile. Everything is OrderBy-sorted so that
    // merely reordering items does not change the fingerprint.
    private static string BuildSignature(UserProfile p)
    {
        var skills = string.Join(",", p.ProfileSkills.Select(ps => ps.Skill.Name).OrderBy(x => x));
        var exp = string.Join(";", p.Experiences
            .Select(e => $"{e.JobTitle}@{e.CompanyName}:{e.StartDate:yyyy-MM}-{(e.EndDate.HasValue ? e.EndDate.Value.ToString("yyyy-MM") : "present")}")
            .OrderBy(x => x));
        var edu = string.Join(";", p.Educations.Select(ed => $"{ed.Degree}@{ed.University}").OrderBy(x => x));
        var certs = string.Join(";", p.Certifications.Select(c => c.Name).OrderBy(x => x));
        var projects = string.Join(";", p.Projects.Select(pr => $"{pr.Title}:{pr.Description}").OrderBy(x => x));
        return $"{p.JobTitle}|{p.Summary}|{skills}|{exp}|{edu}|{certs}|{projects}";
    }
}
