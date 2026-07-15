using Microsoft.EntityFrameworkCore;
namespace CVHack.DAL
{
    public class SupportTicketRepository : GenericRepository<SupportTicket>, ISupportTicketRepository
    {
        public SupportTicketRepository(AppDbContext context) : base(context)
        {
        }

        // USER
        public async Task<IEnumerable<SupportTicket>> GetUserTicketsAsync(string userId)
        {
            return await _context.SupportTickets
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        // ADMIN
        public async Task<IEnumerable<SupportTicket>> GetAllTicketsAsync()
        {
            return await _context.SupportTickets
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SupportTicket?> GetTicketWithUserAsync(int id)
        {
            return await _context.SupportTickets
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

    }
}