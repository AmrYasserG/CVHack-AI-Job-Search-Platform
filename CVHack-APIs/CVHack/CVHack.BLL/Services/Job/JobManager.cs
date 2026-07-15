using CVHack.Common;
using CVHack.DAL;

namespace CVHack.BLL
{
    public class JobManager : IJobManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<JobReadDto>>> GetAllJobsAsync()
        {
            var jobs = await _unitOfWork.JobRepository.GetActiveJobsAsync();
            var result = jobs.Select(MapToDto);

            return Result<IEnumerable<JobReadDto>>.Success(
                result, "Jobs retrieved successfully.", 200);
        }

        public async Task<Result<JobReadDto>> GetJobByIdAsync(int id)
        {
            var job = await _unitOfWork.JobRepository.GetByIdAsync(id);
            if (job is null)
            {
                return Result<JobReadDto>.Failure(
                    "Job not found.", "The requested job does not exist.", 404);
            }

            return Result<JobReadDto>.Success(MapToDto(job), "Job retrieved successfully.", 200);
        }

        private static JobReadDto MapToDto(Job job)
        {
            return new JobReadDto
            {
                Id = job.Id,
                Title = job.Title,
                CompanyName = job.CompanyName,
                City = job.City,
                Country = job.Country,
                Seniority = job.Seniority,
                WorkType = job.WorkType,
                WorkTime = job.WorkTime,
                Description = job.Description,
                BriefDescription = job.BriefDescription,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                PostedAt = job.PostedAt,
                JobUrl = job.JobUrl,
                SourcePlatform = job.SourcePlatform,
                Responsibilities = DeserializeList(job.Responsibilities),
                Requirements = DeserializeList(job.Requirements)
            };
        }

        private static List<string> DeserializeList(string? json) =>
            string.IsNullOrWhiteSpace(json)
                ? new List<string>()
                : (System.Text.Json.JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>());
    }
}