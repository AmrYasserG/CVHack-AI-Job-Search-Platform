namespace CVHack.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ISupportTicketRepository SupportTicketRepository { get; }
        public IApplicationRepository ApplicationRepository { get; }
        public ISavedJobRepository SavedJobRepository { get; }
        public IJobRepository JobRepository { get; }

        public UnitOfWork(AppDbContext context , ISupportTicketRepository supportTicketRepository, IApplicationRepository applicationRepository
            , ISavedJobRepository savedJobRepository, IJobRepository jobRepository)
        {
            _context = context;
            SupportTicketRepository = supportTicketRepository;
            ApplicationRepository = applicationRepository;
            SavedJobRepository = savedJobRepository;
            JobRepository = jobRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
