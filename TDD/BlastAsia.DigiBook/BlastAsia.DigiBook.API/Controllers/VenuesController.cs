using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.JsonPatch;

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
                var venue = this.venueRepository.Retrieve(id.Value);
                result.Add(venue);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateVenue(
            [FromBody] Venue venue)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }
                var result = this.venueService.Save(Guid.Empty, venue);
                return CreatedAtAction("GetContacts",
                  new { id = venue.VenueId }, result);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }

        }

        [HttpDelete]
        public IActionResult DeleteVenue(Guid id)
        {
            var venueToDelete = this.venueRepository.Retrieve(id);
            if (venueToDelete != null)
            {
                this.venueRepository.Delete(id);
                return NoContent();
            }
            return NotFound();
        }
        [HttpPut]
        public IActionResult UpdateVenue(
           [FromBody] Venue venue, Guid id)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }

                var existingVenue = venueRepository.Retrieve(id);
                if (existingVenue == null)
                {
                    return NotFound();
                }
                existingVenue.ApplyChanges(venue);

                var result = this.venueService.Save(id, venue);

                return Ok(existingVenue);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchVenues(
            [FromBody]JsonPatchDocument patchedVenues, Guid id)
        {
            try
            {
                if (patchedVenues == null)
                {
                    return BadRequest();
                }

                var venue = venueRepository.Retrieve(id);
                if (venue == null)
                {
                    return NotFound();
                }

                patchedVenues.ApplyTo(venue);
                venueService.Save(id, venue);

                return Ok(venue);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
