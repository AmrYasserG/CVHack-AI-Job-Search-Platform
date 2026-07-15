using CVHack.Common;

namespace CVHack.BLL
{
    public interface ISupportTicketManager
    {
        // User
        Task<Result<SupportTicketReadDto>> CreateAsync(SupportTicketCreateDto dto, string userId);
        Task<Result<IEnumerable<SupportTicketListDto>>> GetUserTicketsAsync(string userId);
        Task<Result<SupportTicketReadDto>> GetTicketByIdAsync(int id, string userId);

        // Admin
        Task<Result<IEnumerable<AdminSupportTicketListDto>>> GetAllTicketsAsync();
        Task<Result<AdminSupportTicketReadDto>> GetTicketDetailsAsync(int id);
        Task<Result<AdminSupportTicketReadDto>> UpdateTicketStatusAsync(int id, UpdateTicketStatusDto dto);
    }
}