namespace CVHack.DAL
{
    public interface ISupportTicketRepository : IGenericRepository<SupportTicket>
    {
        // USER
        Task<IEnumerable<SupportTicket>> GetUserTicketsAsync(string userId);

        // ADMIN
        Task<IEnumerable<SupportTicket>> GetAllTicketsAsync();
        Task<SupportTicket?> GetTicketWithUserAsync(int id);
    }
}