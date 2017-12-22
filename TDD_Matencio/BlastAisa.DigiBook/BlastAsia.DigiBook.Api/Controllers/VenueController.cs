using System;
using BlastAsia.DigiBook.Domain;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Venues")]
    public class VenueController : Controller
    {
        private readonly IVenueRepository venueRepository;
        private readonly IVenueService venueService;

        public VenueController(IVenueRepository venueRepository, IVenueService venueService)
        {
            this.venueRepository = venueRepository;
            this.venueService = venueService;
        }

        [HttpGet]
        public IActionResult GetVenues(Guid? id)
        {
            var result = new List<Venue>();
            if (id == null)
            {
                result.AddRange(this.venueRepository.Retreive());
            }
            else
            {
                var venue = this.venueRepository.Retrieve(id.Value);
                result.Add(venue);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateVenues(
            [FromBody] Venue venue)
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
        public IActionResult DeleteVenues(Guid id)
        {
            var venueToDelete = this.venueRepository.Retrieve(id);
            if (venueToDelete == null)
            {
                return NotFound();
            }
            this.venueRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateVenues(
            [FromBody] Venue venue, Guid id)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }

                var oldVenue = this.venueRepository.Retrieve(id);
                if (oldVenue == null)
                {
                    return NotFound();
                }

                oldVenue.ApplyChanges(venue);

                var result = this.venueService.Save(id, venue);

                return Ok(venue);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchVenues(JsonPatchDocument patch, Guid id)
        {
            try
            {
                if (patch == null)
                {
                    return BadRequest();
                }
                var venue = venueRepository.Retrieve(id);
                if (venue == null)
                {
                    return NotFound();
                }
                patch.ApplyTo(venue);
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