using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Venues")]
    public class VenuesController : Controller
    {
        private static List<Venue> venues = new List<Venue>();
        private IVenueRepository venueRepository;
        private IVenueService venueService;

        public VenuesController(IVenueRepository venueRepository, IVenueService venueService)
        {
            this.venueRepository = venueRepository;
            this.venueService = venueService;
        }

        [HttpGet, ActionName("GetVenues")]
        public IActionResult GetVenues(Guid? Id)
        {
            List<Venue> result = new List<Venue>();

            if(Id == null) {
                result.AddRange(this.venueRepository.Retrieve());
            }
            else {
                var found = this.venueRepository.Retrieve(Id.Value);
                result.Add(found);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateVenue([FromBody] Venue venue)
        {
            try {
                if(venue == null) {
                    return BadRequest();
                }

                var result = this.venueService.Save(Guid.Empty, venue);

                return CreatedAtAction("GetVanues", new { id = venue.VenueId }, venue);
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteVenue(Guid id)
        {
            var venueToDelete = this.venueRepository.Retrieve(id);
            if(venueToDelete == null) {
                return NotFound();
            }

            this.venueRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateVenue([FromBody] Venue venue, Guid id)
        {
            try {
                if(venue == null) {
                    return BadRequest();
                }

                var oldVenue = this.venueRepository.Retrieve(id);
                if(oldVenue == null) {
                    return NotFound();
                }

                oldVenue.ApplyNewChanges(venue);
                var result = this.venueService.Save(id, oldVenue);

                return Ok(oldVenue);
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [HttpPatch]
        public object PatchVenue([FromBody]JsonPatchDocument patchedVenue, Guid venueId)
        {
            try {
                if(patchedVenue == null) {
                    return BadRequest();
                }

                var venue = this.venueRepository.Retrieve(venueId);
                if(venue == null) {
                    return NotFound();
                }

                patchedVenue.ApplyTo(venue);
                venueService.Save(venue.VenueId, venue);

                return Ok(venue);

            }
            catch (Exception) {
                return BadRequest();
            }
        }
    }
}