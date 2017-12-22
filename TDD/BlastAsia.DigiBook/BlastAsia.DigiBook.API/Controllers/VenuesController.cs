using System;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Models.Venues;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Venues")]
    public class VenuesController : Controller
    {
        private IVenueRepository venueRepository;
        private IVenueService venueService;

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
                var venue = this.venueRepository.Retrieve(id.Value);
                result.Add(venue);
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateVenue(
            [FromBody]Venue venue)
        {
            try
            {
               
                if (venue == null)
                {
                    return BadRequest();
                }

                var result = this.venueService.Save(Guid.Empty, venue);

                return CreatedAtAction("GetVenues",
                     new { id = venue.VenueId }, result); 
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteVenue(Guid id)
        {
            var venueToDelete = this.venueRepository.Retrieve(id);
            if (venueToDelete == null)
            {
                return NotFound();
            }

            venueRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateVenue(
            [FromBody] Venue venue, Guid id)
        {
            if (venue == null)
            {
                return BadRequest();
            }

            var venueToUpdate = venueRepository.Retrieve(id);
            if (venueToUpdate == null)
            {
                return NotFound();
            }
            venueToUpdate.ApplyChanges(venue);
            this.venueRepository.Update(id, venueToUpdate);
            return Ok(venue);
        }

        [HttpPatch]
        public IActionResult PatchVenue(
            [FromBody]JsonPatchDocument patchedVenue, Guid id)
        {
            if (patchedVenue == null)
            {
                return BadRequest();
            }

            var venue = venueRepository.Retrieve(id);
            if (venue == null)
            {
                return NotFound();
            }

            patchedVenue.ApplyTo(venue);
            venueService.Save(id, venue);

            return Ok(venue);
        }
    }
}