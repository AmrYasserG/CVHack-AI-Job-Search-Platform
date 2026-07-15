using CVHack.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVHack.API
{
    [Route("api/jobs")]
    [ApiController]
    [Authorize]   // any logged-in user can browse jobs
    public class JobsController : ControllerBase
    {
        private readonly IJobManager _jobManager;
        private readonly ICompanyResearchService _companyResearchService;
        private readonly ISkillAnalysisService _skillAnalysisService;

        public JobsController(
            IJobManager jobManager,
            ICompanyResearchService companyResearchService,
            ISkillAnalysisService skillAnalysisService)               
        {
            _jobManager = jobManager;
            _companyResearchService = companyResearchService;
            _skillAnalysisService = skillAnalysisService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _jobManager.GetAllJobsAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _jobManager.GetJobByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id:int}/briefing")]
        public async Task<IActionResult> GetBriefing(int id)
        {
            var result = await _companyResearchService.GetBriefingAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("{id:int}/skill-analysis")]
        public async Task<IActionResult> GetSkillAnalysis(int id)
        {
            var result = await _skillAnalysisService.AnalyzeAsync(id, User.GetUserId());
            return StatusCode(result.StatusCode, result);
        }
    }
}