using CVHack.Common;

namespace CVHack.BLL.Services.Profile;

public interface IProfileService
{
    Task<Result<ProfileResponseDto>> GetProfileAsync(string userId);
    Task<Result<ProfileResponseDto>> UpdateProfileAsync(string userId, UpdateProfileDto dto);
}
