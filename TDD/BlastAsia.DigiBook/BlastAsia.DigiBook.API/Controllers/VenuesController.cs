using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Venues.Service;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.JsonPatch;

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

        [HttpGet, ActionName("GetVenues")]
        public IActionResult GetVenues(Guid? guid)
        {
            var venueResults = new List<Venue>();
            if (guid == null)
            {
                venueResults.AddRange(this.venueRepository.Retrieve());
            }
            else
            {
                var retrievedVenue = venueRepository.Retrieve(guid.Value);
                venueResults.Add(retrievedVenue);
            }

            return Ok(venueResults);
        }

        [HttpPost]
        public IActionResult PostVenue([FromBody] Venue venue)
        {
            try
            {
                if (venue == null) return BadRequest();

                var result = this.venueService.Save(Guid.Empty, venue);
                return CreatedAtAction("GetVenues", new { id = venue.VenueId, result });
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteVenue(Guid id)
        {
            var retrievedResult = venueRepository.Retrieve(id);
            if (retrievedResult == null) return NotFound();

            this.venueRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateVenue([FromBody] Venue venue, Guid id)
        {
            try
            {
                var existingVenue = this.venueRepository.Retrieve(id);
                existingVenue.ApplyChanges(venue);

                this.venueService.Save(id, venue);
                return Ok();

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchVenue([FromBody]JsonPatchDocument patchVenue, Guid id)
        {
            if (patchVenue == null) return BadRequest();

            var venue = venueRepository.Retrieve(id);
            if (venue == null) return NotFound();

            return Ok();
        }
    }
}