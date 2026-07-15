using CVHack.Common;

namespace CVHack.BLL.Services.AdminUser
{
    public interface IAdminUserService
    {
        Task<Result<IEnumerable<AdminUserListDto>>> GetAllUsersAsync();
        Task<Result<AdminUserListDto>> GetUserByIdAsync(string id);
        Task<Result<AdminUserListDto>> PromoteToAdminAsync(string id);
        Task<Result<AdminUserListDto>> UpdateStatusAsync(string id, string status);
        Task<Result<AdminUserListDto>> UpdatePlanAsync(string id, string plan);
    }
}