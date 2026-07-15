using CVHack.Common;

namespace CVHack.BLL;

public interface ICompanyResearchService
{
    Task<Result<CompanyBriefingDto>> GetBriefingAsync(int jobId);
}