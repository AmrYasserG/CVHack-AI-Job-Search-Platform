using CVHack.BLL.Services.Certification;
using CVHack.Common;
using CVHack.DAL;

namespace CVHack.BLL;

public class CertificationService : ICertificationService
{
    private readonly ICertificationRepository _certificationRepository;
    private readonly IProfileRepository _profileRepository;

    public CertificationService(
        ICertificationRepository certificationRepository,
        IProfileRepository profileRepository)
    {
        _certificationRepository = certificationRepository;
        _profileRepository = profileRepository;
    }

    public async Task<Result<List<CertificationResponseDto>>> GetAllAsync(string userId)
    {
        var certifications = await _certificationRepository.GetAllByUserIdAsync(userId);
        var dtos = certifications.Select(MapToDto).ToList();

        return Result<List<CertificationResponseDto>>.Success(dtos);
    }

    public async Task<Result<CertificationResponseDto>> CreateAsync(string userId, CertificationDto dto)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
        {
            return Result<CertificationResponseDto>.Failure("Profile not found.", statusCode: 404);
        }

        var certification = new Certification
        {
            ProfileId = profile.Id,
            Name = dto.Name,
            Provider = dto.Provider,
            CredentialUrl = dto.CredentialUrl,
            CertifiedAt = dto.CertifiedAt
        };

        await _certificationRepository.AddAsync(certification);
        await _certificationRepository.SaveChangesAsync();

        return Result<CertificationResponseDto>.Success(MapToDto(certification), "Certification added successfully.", 201);
    }

    public async Task<Result<CertificationResponseDto>> UpdateAsync(string userId, int id, CertificationDto dto)
    {
        var certification = await _certificationRepository.GetByIdAsync(id);
        if (certification == null)
        {
            return Result<CertificationResponseDto>.Failure("Certification not found.", statusCode: 404);
        }

        if (certification.Profile.UserId != userId)
        {
            return Result<CertificationResponseDto>.Failure("You do not have access to this certification.", statusCode: 403);
        }

        certification.Name = dto.Name;
        certification.Provider = dto.Provider;
        certification.CredentialUrl = dto.CredentialUrl;
        certification.CertifiedAt = dto.CertifiedAt;

        _certificationRepository.Update(certification);
        await _certificationRepository.SaveChangesAsync();

        return Result<CertificationResponseDto>.Success(MapToDto(certification), "Certification updated successfully.");
    }

    public async Task<Result<bool>> DeleteAsync(string userId, int id)
    {
        var certification = await _certificationRepository.GetByIdAsync(id);
        if (certification == null)
        {
            return Result<bool>.Failure("Certification not found.", statusCode: 404);
        }

        if (certification.Profile.UserId != userId)
        {
            return Result<bool>.Failure("You do not have access to this certification.", statusCode: 403);
        }

        _certificationRepository.Delete(certification);
        await _certificationRepository.SaveChangesAsync();

        return Result<bool>.Success(true, "Certification deleted successfully.");
    }

    private static CertificationResponseDto MapToDto(Certification certification)
    {
        return new CertificationResponseDto
        {
            Id = certification.Id,
            Name = certification.Name,
            Provider = certification.Provider,
            CredentialUrl = certification.CredentialUrl,
            CertifiedAt = certification.CertifiedAt
        };
    }
}
