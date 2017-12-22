using BlastAsia.DigiBook.Domain.Venues;

namespace BlastAsia.DigiBook.API.Controllers
{
    public class VenuesController
    {
        private readonly IVenueService venueService;
        private readonly IVenueRepository venueRepository;

        public VenuesController(IVenueService venueService, IVenueRepository venueRepository)
        {
            this.venueService = venueService;
            this.venueRepository = venueRepository;
        }
    }
}