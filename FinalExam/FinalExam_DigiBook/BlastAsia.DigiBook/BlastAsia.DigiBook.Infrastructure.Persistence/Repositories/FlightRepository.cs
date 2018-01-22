using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models.Flights;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class FlightRepository
        : RepositoryBase<Flight>, IFlightRepository
    {
        private DigiBookDbContext dbContext;

        public FlightRepository(IDigiBookDbContext context)
            : base(context)
        {
        }
    }
}