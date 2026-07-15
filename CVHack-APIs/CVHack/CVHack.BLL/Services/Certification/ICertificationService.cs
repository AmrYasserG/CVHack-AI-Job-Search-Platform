using CVHack.Common;

namespace CVHack.BLL.Services.Certification;

public interface ICertificationService
{
    Task<Result<List<CertificationResponseDto>>> GetAllAsync(string userId);
    Task<Result<CertificationResponseDto>> CreateAsync(string userId, CertificationDto dto);
    Task<Result<CertificationResponseDto>> UpdateAsync(string userId, int id, CertificationDto dto);
    Task<Result<bool>> DeleteAsync(string userId, int id);
}
