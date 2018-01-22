using BlastAsia.DigiBook.Domain.Locations;
using BlastAsia.DigiBook.Domain.Models.Locations;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class LocationRepository 
        : RepositoryBase<Location>, ILocationRepository
    {

        public LocationRepository(IDigiBookDbContext context)
            : base(context)
        {
            
        }
    }
}