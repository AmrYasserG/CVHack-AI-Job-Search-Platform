using CVHack.Common;
using CVHack.DAL;
using FluentValidation;

namespace CVHack.BLL
{ 
  public class ApplicationManager : IApplicationManager
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ApplicationCreateDto> _validator;

    public ApplicationManager(
        IUnitOfWork unitOfWork,
        IValidator<ApplicationCreateDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<ApplicationReadDto>> CreateAsync(ApplicationCreateDto dto, string userId)
    {
        #region Validation

        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            return Result<ApplicationReadDto>.Failure(
                errors,
                "One or more validation errors occurred.", 
                400);
        }

        #endregion

        #region Check Job Exists

        var job = await _unitOfWork.JobRepository.GetByIdAsync(dto.JobId);

          if (job is null)
          {
            return Result<ApplicationReadDto>.Failure(
                "Job not found.",
                "The requested job does not exist.",
                404);
          }

           //if (!job.IsActive)
           //{
           //     return Result<ApplicationReadDto>.Failure(
           //         "This job is no longer available.",
           //         "The job is inactive.",
           //         400);
           //}

            #endregion

            #region Check Duplicate Application

            bool hasApplied = await _unitOfWork.ApplicationRepository
            .HasUserAppliedAsync(userId, dto.JobId);

        if (hasApplied)
        {
            return Result<ApplicationReadDto>.Failure(
                "You have already applied for this job.",
                "Duplicate application.",
                409);
        }

        #endregion

        #region Create Application

        var application = new Application
        {
            UserId = userId,
            JobId = dto.JobId,
            MatchScore = 0,
            MockInterview = false,
            AppliedAt = DateTime.UtcNow
        };

        _unitOfWork.ApplicationRepository.Insert(application);

        await _unitOfWork.SaveChangesAsync();

        #endregion

        #region Mapping

        var applicationDto = new ApplicationReadDto
        {
            ApplicationId= application.Id,
            JobId = application.JobId,
            JobTitle = job.Title,
            CompanyName = job.CompanyName,
            MatchScore = application.MatchScore,
            MockInterview = application.MockInterview,
            AppliedAt = application.AppliedAt
        };

        #endregion

        return Result<ApplicationReadDto>.Success(
            applicationDto,
            "Application submitted successfully.",
            201);
    }

    public async Task<Result<IEnumerable<ApplicationReadDto>>> GetUserApplicationsAsync(string userId)
    {
        var applications = await _unitOfWork.ApplicationRepository.GetUserApplicationsAsync(userId);

        var result = applications.Select(a => new ApplicationReadDto
        {
            ApplicationId = a.Id,
            JobId = a.JobId,
            JobTitle = a.Job.Title,
            CompanyName = a.Job.CompanyName,
            MatchScore = a.MatchScore,
            MockInterview = a.MockInterview,
            AppliedAt = a.AppliedAt
        });

        return Result<IEnumerable<ApplicationReadDto>>.Success(
            result,
            "Applications retrieved successfully.",
            200);
    }

  }
}