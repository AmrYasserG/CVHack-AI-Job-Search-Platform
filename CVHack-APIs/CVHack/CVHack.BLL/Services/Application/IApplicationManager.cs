using CVHack.Common;

namespace CVHack.BLL
{
   public interface IApplicationManager
   {
      Task<Result<ApplicationReadDto>> CreateAsync(ApplicationCreateDto dto, string userId);
      Task<Result<IEnumerable<ApplicationReadDto>>> GetUserApplicationsAsync(string userId);
   }
}