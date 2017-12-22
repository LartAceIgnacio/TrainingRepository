using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Venues")]
    public class VenuesController : Controller
    {
        private readonly IVenueService venueService;
        private readonly IVenueRepository venueRepository;

        public VenuesController(IVenueRepository venueRepository, IVenueService venueService)
        {
            this.venueRepository = venueRepository;
            this.venueService = venueService;
        }

        [HttpGet, ActionName("GetVenues")]
        public IActionResult GetVenues(Guid? id)
        {
            var result = new List<Venue>();

            if (id == null)
            {
                result.AddRange(this.venueRepository.Retrieve());
            }
            else
            {
                var venue = venueRepository.Retrieve(id.Value);
                result.Add(venue);
            }
            return Ok(result);
        }

        public IActionResult CreateVenue([FromBody]Venue venue)
        {
            var result = this.venueService.Save(venue);

            return CreatedAtAction("GetVenues", new { id = venue.VenueId }, venue);
        }
    }
}