using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class PilotRepository 
        : RepositoryBase<Pilot>, IPilotRepository
    {
        public PilotRepository(IDigiBookDbContext context) : base(context)
        {

        }
    }
}