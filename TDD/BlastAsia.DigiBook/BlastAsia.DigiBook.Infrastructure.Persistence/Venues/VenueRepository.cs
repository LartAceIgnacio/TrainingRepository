using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Venues
{
    public class VenueRepository
        :RepositoryBase<Venue>, IVenueRepository
    {
        public VenueRepository(IDigiBookDbContext context):
            base(context)
        {

        } 
    }
}