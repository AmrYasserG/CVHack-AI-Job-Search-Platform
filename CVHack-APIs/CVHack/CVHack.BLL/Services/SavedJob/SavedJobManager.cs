using CVHack.Common;
using CVHack.DAL;
using FluentValidation;

namespace CVHack.BLL
{
    public class SavedJobManager : ISavedJobManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SavedJobCreateDto> _validator;

        public SavedJobManager(
            IUnitOfWork unitOfWork,
            IValidator<SavedJobCreateDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result<SavedJobReadDto>> SaveJobAsync(SavedJobCreateDto dto, string userId)
        {
            #region Validation

            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Result<SavedJobReadDto>.Failure(
                    errors,
                    "One or more validation errors occurred.",
                    400);
            }

            #endregion

            #region Check Job Exists

            var job = await _unitOfWork.JobRepository.GetByIdAsync(dto.JobId);

            if (job is null)
            {
                return Result<SavedJobReadDto>.Failure(
                    "Job not found.",
                    "The requested job does not exist.",
                    404);
            }

            //if (!job.IsActive)
            //{
            //    return Result<SavedJobReadDto>.Failure(
            //        "Job is no longer available.",
            //        "This job is inactive.",
            //        400);
            //}

            #endregion

            #region Check Duplicate Save

            bool isSaved = await _unitOfWork.SavedJobRepository.IsJobSavedAsync(userId, dto.JobId);

            if (isSaved)
            {
                return Result<SavedJobReadDto>.Failure(
                    "Job already saved.",
                    "Duplicate saved job.",
                    409);
            }

            #endregion

            #region Create Saved Job

            var savedJob = new SavedJob
            {
                UserId = userId,
                JobId = dto.JobId,
                SavedAt = DateTime.UtcNow
            };

            _unitOfWork.SavedJobRepository.Insert(savedJob);

            await _unitOfWork.SaveChangesAsync();

            #endregion

            #region Mapping

            var result = new SavedJobReadDto
            {
                JobId = savedJob.JobId,
                JobTitle = job.Title,
                CompanyName = job.CompanyName,
                Location = $"{job.City}, {job.Country}",
                JobUrl = job.JobUrl,
                SavedAt = savedJob.SavedAt
            };

            #endregion

            return Result<SavedJobReadDto>.Success(
                result,
                "Job saved successfully.",
                201);
        }

        public async Task<Result<IEnumerable<SavedJobReadDto>>> GetUserSavedJobsAsync(string userId)
        {
            var savedJobs = await _unitOfWork.SavedJobRepository.GetUserSavedJobsAsync(userId);

            var result = savedJobs.Select(s => new SavedJobReadDto
            {
                JobId = s.JobId,
                JobTitle = s.Job.Title,
                CompanyName = s.Job.CompanyName,
                Location = $"{s.Job.City}, {s.Job.Country}",
                JobUrl = s.Job.JobUrl,
                SavedAt = s.SavedAt
            });

            return Result<IEnumerable<SavedJobReadDto>>.Success(
                result,
                "Saved jobs retrieved successfully.",
                200);
        }

        public async Task<Result<bool>> RemoveSavedJobAsync(int jobId, string userId)
        {
            var savedJob = await _unitOfWork.SavedJobRepository.GetByIdAsync(userId, jobId);

            if (savedJob is null)
            {
                return Result<bool>.Failure(
                    "Saved job not found.",
                    "The requested saved job does not exist.",
                    404);
            }

            _unitOfWork.SavedJobRepository.Delete(savedJob);

            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(
                true,
                "Job removed from saved jobs successfully.",
                200);
        }

    }
}