using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Api.Utils;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Venues")]
    public class VenuesController : Controller
    {
        private IVenueService venueService;
        private IVenueRepository venueRepo;

        public VenuesController(IVenueService venueService, IVenueRepository venueRepo)
        {
            this.venueService = venueService;
            this.venueRepo = venueRepo;
        }

        [HttpGet,ActionName("GetVenues")]
        public IActionResult GetVenues(Guid? id)
        {
            try
            {
                var result = new List<Venue>();
                if (id == null)
                {
                    result.AddRange(this.venueRepo.Retrieve());
                }
                else
                {
                    var venue = this.venueRepo.Retrieve(id.Value);
                    result.Add(venue);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult CreateVenue([FromBody] Venue venue)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }
                else
                {
                    this.venueService.Save(Guid.Empty, venue);
                    return CreatedAtAction("GetVenues", new { id = venue.VenueId }, venue);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPut]
        public IActionResult UpdateVenue([FromBody]Guid venueId, Venue venue)
        {
            try
            {
                var existingVenue = this.venueRepo.Retrieve(venueId);
                if (existingVenue == null)
                {
                    return NotFound();
                }
                else
                {
                    existingVenue.ApplyChanges(venue);

                    this.venueService.Save(venueId, existingVenue);
                    return Ok(venue);
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }


        }
        [HttpDelete]
        public IActionResult DeleteVenue(Guid venueId, Venue venue)
        {
            try
            {
                var existingVenue = this.venueRepo.Retrieve(venueId);

                if (existingVenue == null)
                {
                    return NotFound();
                }
                else
                {
                    this.venueRepo.Delete(venueId);
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
          
        }

        [HttpPatch]
        public object PatchVenue([FromBody] JsonPatchDocument patchDoc, Guid venueId)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest();
                }
                var existingVenue = this.venueRepo.Retrieve(venueId);
                if (existingVenue == null)
                {
                    return NotFound();
                }

                else
                {
                    patchDoc.ApplyTo(existingVenue);
                    this.venueService.Save(venueId, existingVenue);
                    return Ok(existingVenue);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}