using CVHack.BLL.Services.AdminUser;
using CVHack.BLL.Services.Auth;
using CVHack.BLL.Services.Certification;
using CVHack.BLL.Services.CvGenerator;
using CVHack.BLL.Services.Education;
using CVHack.BLL.Services.Experience;
using CVHack.BLL.Services.Profile;
using CVHack.DAL;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CVHack.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccess(configuration);

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IProfileSkillService, ProfileSkillService>();
        services.AddScoped<IProjectService, ProjectService>();

        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IExperienceService, ExperienceService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<ICertificationService, CertificationService>();

        services.AddScoped<IApplicationManager, ApplicationManager>();
        services.AddScoped<ISupportTicketManager, SupportTicketManager>();
        services.AddScoped<IAdminUserService, AdminUserService>();
        services.AddScoped<ISavedJobManager, SavedJobManager>();
        services.AddScoped<IJobManager, JobManager>();

        services.AddScoped<ICompanyResearchService, CompanyResearchService>();
        services.AddScoped<ISkillAnalysisService, SkillAnalysisService>();

        services.AddScoped<ICvGeneratorService, CvGeneratorService>();

        services.AddScoped<IJobIngestionService, JobIngestionService>();

        services.AddSingleton<JobIngestionHostedService>();
        services.AddHostedService(sp => sp.GetRequiredService<JobIngestionHostedService>());

        return services;
    }
}
