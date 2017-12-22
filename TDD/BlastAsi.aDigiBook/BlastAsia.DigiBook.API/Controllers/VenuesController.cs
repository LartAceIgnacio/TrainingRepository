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
        public IActionResult CreateVenue([FromBody] Venue venue)
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

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var venueToDelete = this.venueRepository.Retrieve(id);
            if (venueToDelete != null)
            {
                this.venueRepository.Delete(id);
                return NoContent();
            }
            return NotFound();
        }

        [HttpPatch]
        public IActionResult PatchVenue([FromBody] JsonPatchDocument patchedContact, Guid id)
        {
            try
            {
                if(patchedContact == null)
                {
                    return BadRequest();
                }

                var venue = venueRepository.Retrieve(id);
                if (venue == null)
                {
                    return NotFound();
                }

                patchedContact.ApplyTo(venue);
                venueService.Save(id, venue);
                return Ok(venue);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateVenue( [FromBody] Venue venue, Guid venueId)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }

                var oldVenue = this.venueRepository.Retrieve(venueId);
                if (oldVenue == null)
                {
                    return NotFound();
                }

                oldVenue.ApplyChanges(venue);

                var result = this.venueService.Save(venueId, venue);

                return Ok(oldVenue);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}