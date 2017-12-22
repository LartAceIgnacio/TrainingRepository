using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.Digibook.API.Utils;
using BlastAsia.Digibook.Domain.Models.Venues;
using BlastAsia.Digibook.Domain.Venues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.Digibook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Venue")]
    public class VenueController : Controller
    {
        private IVenueRepository venueRepository;
        private IVenueService venueService;

        public VenueController(IVenueRepository venueRepository, IVenueService venueService)
        {
            this.venueRepository = venueRepository;
            this.venueService = venueService;
        }

        [HttpGet,ActionName("GetVenues")]
        public IActionResult GetVenue(Guid? id)
        {
            List<Venue> result = new List<Venue>();

            if (id == null)
            {
                result.AddRange(this.venueRepository.Retrieve());
            }
            else
            {
                result.Add(this.venueRepository.Retrieve(id.Value));
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
                this.venueService.Save(Guid.Empty, venue);
                return CreatedAtAction("GetVenues", new { id = venue.VenueId }, venue);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteVenue(Guid id)
        {
            var contactToDelete = this.venueRepository.Retrieve(id);
            if (contactToDelete != null)
            {
                this.venueRepository.Delete(id);
            }
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateVenue(
            [FromBody] Venue venue,Guid id)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }

                var oldContact = this.venueRepository.Retrieve(id);
                if (oldContact == null)
                {
                    return NotFound();
                }

                oldContact.ApplyVenueChanges(venue);

                var result = this.venueService.Save(id, venue);

                return Ok(oldContact);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        public IActionResult PatchVenue(
           [FromBody]JsonPatchDocument patchedContact, Guid id)
        {
            if (patchedContact == null)
            {
                return BadRequest();
            }
            var contact = venueRepository.Retrieve(id);
            if (contact == null)
            {
                return NotFound();
            }
            patchedContact.ApplyTo(contact);
            venueService.Save(id, contact);

            return Ok(contact);
        }
    }
}