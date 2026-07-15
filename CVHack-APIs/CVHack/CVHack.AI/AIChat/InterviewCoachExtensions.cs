using Microsoft.Extensions.DependencyInjection;

namespace CVHack.AI.AIChat;

// تسجيل خدمات Worker 5 (Interview Coach) في مكان واحد
public static class InterviewCoachExtensions
{
    public static IServiceCollection AddInterviewCoach(this IServiceCollection services)
    {
        services.AddScoped<InterviewQuestionBank>();
        services.AddSingleton<InterviewSessionStore>();
        services.AddScoped<InterviewCoachAgent>();
        return services;
    }
}