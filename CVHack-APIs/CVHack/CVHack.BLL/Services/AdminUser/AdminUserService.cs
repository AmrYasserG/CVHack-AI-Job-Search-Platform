using CVHack.Common;
using CVHack.DAL;
using Microsoft.AspNetCore.Identity;

namespace CVHack.BLL.Services.AdminUser
{
    public class AdminUserService : IAdminUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminUserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<IEnumerable<AdminUserListDto>>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var result = new List<AdminUserListDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(MapToDto(user, roles));
            }

            return Result<IEnumerable<AdminUserListDto>>.Success(
                result, "Users retrieved successfully.", 200);
        }

        public async Task<Result<AdminUserListDto>> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return Result<AdminUserListDto>.Failure(
                    "User not found.", "The requested user does not exist.", 404);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Result<AdminUserListDto>.Success(
                MapToDto(user, roles), "User retrieved successfully.", 200);
        }

        private static AdminUserListDto MapToDto(ApplicationUser user, IList<string> roles)
        {
            return new AdminUserListDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Roles = roles,
                Plan = user.Plan,
                Status = user.Status,
                Searches = user.Searches,
                CreatedAt = user.CreatedAt
            };
        }
        public async Task<Result<AdminUserListDto>> PromoteToAdminAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return Result<AdminUserListDto>.Failure("User not found.", "The requested user does not exist.", 404);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return Result<AdminUserListDto>.Failure("User is already an admin.", "Operation not allowed.", 400);

            var addResult = await _userManager.AddToRoleAsync(user, "Admin");
            if (!addResult.Succeeded)
            {
                var errors = addResult.Errors.Select(e => e.Description).ToList();
                return Result<AdminUserListDto>.Failure(errors, "Failed to promote user.", 500);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Result<AdminUserListDto>.Success(MapToDto(user, roles), "User promoted to admin.", 200);
        }

        public async Task<Result<AdminUserListDto>> UpdateStatusAsync(string id, string status)
        {
            if (status != "Active" && status != "Suspended")
                return Result<AdminUserListDto>.Failure("Invalid status.", "Status must be 'Active' or 'Suspended'.", 400);

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return Result<AdminUserListDto>.Failure("User not found.", "The requested user does not exist.", 404);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return Result<AdminUserListDto>.Failure("Cannot change the status of an admin.", "Operation not allowed on admins.", 400);

            user.Status = status;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            return Result<AdminUserListDto>.Success(MapToDto(user, roles), "User status updated.", 200);
        }

        public async Task<Result<AdminUserListDto>> UpdatePlanAsync(string id, string plan)
        {
            if (plan != "Free" && plan != "Pro")
                return Result<AdminUserListDto>.Failure("Invalid plan.", "Plan must be 'Free' or 'Pro'.", 400);

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return Result<AdminUserListDto>.Failure("User not found.", "The requested user does not exist.", 404);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return Result<AdminUserListDto>.Failure("Cannot change the plan of an admin.", "Operation not allowed on admins.", 400);

            user.Plan = plan;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            return Result<AdminUserListDto>.Success(MapToDto(user, roles), "User plan updated.", 200);
        }
    }
}