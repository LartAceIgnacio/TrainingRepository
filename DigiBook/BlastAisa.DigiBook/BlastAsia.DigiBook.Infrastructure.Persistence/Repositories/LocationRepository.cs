using BlastAsia.DigiBook.Domain.Locations;
using BlastAsia.DigiBook.Domain.Models.Locations;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        private DigiBookDbContext context;

        public LocationRepository(DigiBookDbContext context) : base (context)
        {
            this.context = context;
        }
    }
}