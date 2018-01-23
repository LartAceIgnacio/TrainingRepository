using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class PilotRepository
    {
        private DigiBookDbContext context;

        public PilotRepository(DigiBookDbContext context)
        {
            this.context = context;
        }
        public Pilot Create(Pilot pilot)
        {
            context.Set<Pilot>().Add(pilot);
            context.SaveChanges();
            return pilot;
        }
    }
}