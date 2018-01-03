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
using Microsoft.AspNetCore.Cors;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
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
                var venue = this.venueRepository.Retrieve(id.Value);
                result.Add(venue);
            }
            return Ok(result);
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
                var result = this.venueService.Save(Guid.Empty, venue);

                return CreatedAtAction("GetVenues", new { id = venue.VenueId }, venue);
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
            this.venueRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateVenue([FromBody]Venue venue, Guid id)
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

                var result = this.venueService.Save(id, existingVenue);

                return Ok(venue);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchVenue([FromBody]JsonPatchDocument patchVenue, Guid id)
        {

            if (patchVenue == null)
            {
                return BadRequest();
            }

            var venue = venueRepository.Retrieve(id);

            if (venue == null)
            {
                return NotFound();
            }

            patchVenue.ApplyTo(venue);
            venueService.Save(id, venue);

            return Ok(venue);
        }
    }
}