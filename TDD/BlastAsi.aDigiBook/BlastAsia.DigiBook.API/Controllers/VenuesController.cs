using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Venues")]
    public class VenuesController : Controller
    {
        private IVenueService venueService;
        private IVenueRepository venueRepository;

        public VenuesController(IVenueService venueService, IVenueRepository venueRepository)
        {
            this.venueService = venueService;
            this.venueRepository = venueRepository;
        }

        [HttpGet, ActionName("GetVenue")]
        public IActionResult GetVenue(Guid? id)
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

        [HttpPost]
        public object CreateVenue(Venue venue)
        {
            try
            {
                if(venue == null)
                {
                    return BadRequest();
                }

                var result = this.venueService.Save(Guid.Empty, venue);
                return CreatedAtAction("GetVenues", new { id = result.VenueId }, result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}