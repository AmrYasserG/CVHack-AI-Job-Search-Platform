using CVHack.AI.AIChat;
using CVHack.Common;

namespace CVHack.API;

// ربط endpoints الخاصة بـ Worker 5 (Interview Coach) بالتطبيق
public static class InterviewCoachEndpoints
{
    public static WebApplication MapInterviewCoachEndpoints(this WebApplication app)
    {
        app.MapPost("/api/interview/start", async (InterviewCoachAgent agent, InterviewStartRequest request) =>
        {
            var result = await agent.StartAsync(request);
            return Results.Ok(Result<InterviewTurnResult>.Success(result));
        }).RequireAuthorization();

        app.MapPost("/api/interview/answer", async (InterviewCoachAgent agent, InterviewAnswerRequest request) =>
        {
            var result = await agent.AnswerAsync(request);
            return Results.Ok(Result<InterviewTurnResult>.Success(result));
        }).RequireAuthorization();

        return app;
    }
}