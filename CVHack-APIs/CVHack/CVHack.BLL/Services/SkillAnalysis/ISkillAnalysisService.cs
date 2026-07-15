using CVHack.Common;

namespace CVHack.BLL;

public interface ISkillAnalysisService
{
    Task<Result<SkillAnalysisDto>> AnalyzeAsync(int jobId, string userId);
}