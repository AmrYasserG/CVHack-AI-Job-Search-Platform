using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CVHack.DAL;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<ProfileSkill> ProfileSkills => Set<ProfileSkill>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<Certification> Certifications => Set<Certification>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<SkillGapAnalysis> SkillGapAnalyses => Set<SkillGapAnalysis>();
    public DbSet<SkillGapItem> SkillGapItems => Set<SkillGapItem>();
    public DbSet<SavedJob> SavedJobs => Set<SavedJob>();

    public DbSet<CompanyBriefing> CompanyBriefings => Set<CompanyBriefing>();
    public DbSet<SupportedRole> SupportedRoles => Set<SupportedRole>();


}