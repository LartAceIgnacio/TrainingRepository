using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class VenuesController : Controller
    {
        private readonly IVenueService venueService;
        private readonly IVenueRepository venueRepository;

        public VenuesController(IVenueService venueService,
            IVenueRepository venueRepository)
        {
            this.venueService = venueService;
            this.venueRepository = venueRepository;

        }
    }
}
