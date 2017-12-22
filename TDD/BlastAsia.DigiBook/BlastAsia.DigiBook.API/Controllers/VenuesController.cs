using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult CreateVenue([FromBody]Venue venue)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }
                var result = this.venueService.Save(venue.VenueId, venue);

                return CreatedAtAction("GetVenues", new { id = venue.VenueId }, venue);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteVenue(Guid venueId)
        {
            var venueToDelete = this.venueRepository.Retrieve(venueId);
            if (venueToDelete == null)
            {
                return NotFound();
            }
            this.venueRepository.Delete(venueId);

            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateVenue([FromBody]Venue venue, Guid venueId)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }

                var existingVenue = venueRepository.Retrieve(venueId);
                if (existingVenue == null)
                {
                    return NotFound();
                }
                existingVenue.ApplyChanges(venue);

                this.venueService.Save(venueId, existingVenue);

                return Ok(venue);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchVenue([FromBody]JsonPatchDocument patchVenue, Guid venueId)
        {

            if (patchVenue == null)
            {
                return BadRequest();
            }

            var venue = venueRepository.Retrieve(venueId);

            if (venue == null)
            {
                return NotFound();
            }

            patchVenue.ApplyTo(venue);
            venueService.Save(venueId, venue);

            return Ok(venue);
        }
    }
}