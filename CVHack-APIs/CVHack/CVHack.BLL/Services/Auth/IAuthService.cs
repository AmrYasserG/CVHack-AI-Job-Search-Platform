using CVHack.BLL.DTOs.Auth;
using CVHack.Common;
using System.Threading.Tasks;

namespace CVHack.BLL.Services.Auth;

public interface IAuthService
{
    Task<Result<AuthResultDto>> RegisterJobSeekerAsync(RegisterJobSeekerDto dto);
    Task<Result<AuthResultDto>> LoginAsync(LoginDto dto);
}
