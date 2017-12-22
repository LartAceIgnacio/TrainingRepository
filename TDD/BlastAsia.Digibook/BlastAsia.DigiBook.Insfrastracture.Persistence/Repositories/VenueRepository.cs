using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Insfrastracture.Persistence;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Repositories
{
    public class VenueRepository
          : RepositoryBase<Venue>, IVenueRepository
    {
        public VenueRepository(IDigiBookDbContext context)
            : base(context)
        {

        }
    }
}