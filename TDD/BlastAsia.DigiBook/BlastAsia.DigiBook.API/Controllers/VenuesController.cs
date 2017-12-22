using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Models.Venues;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.API.Utils;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Venues")]
    public class VenuesController : Controller
    {
        private readonly IVenueService venueService;
        private readonly IVenueRepository venueRepository;

        public VenuesController(
            IVenueRepository venueRepository
            , IVenueService venueService)
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
                result.AddRange(
                    this.venueRepository.Retrieve());
            }
            else
            {
                var venue = this.venueRepository.Retrieve
                    (id.Value);
                result.Add(venue);
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateVenue(
            [Bind] Venue venue)
        {
            try
            {
                if(venue == null)
                {
                    return BadRequest();
                }
                var result = this.venueService
                    .Save(Guid.Empty, venue);

                return CreatedAtAction("GetVenues", new { id = venue.VenueID }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult UpdateVenue(
            [FromBody] Venue newVenue, Guid id)
        {
            var venue = venueRepository.Retrieve(id);
            if(venue == null)
            {
                return BadRequest();
            }
            venue.ApplyChanges(newVenue);
            venueService.Save(id, venue);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteVenue(Guid id)
        {
            var deletedVenue = venueRepository.Retrieve(id);
            if (deletedVenue == null)
            {
                return NotFound();
            }
            this.venueRepository.Delete(id);

            return Ok();
        }

        [HttpPatch]
        public IActionResult PatchVenue(
            [FromBody] JsonPatchDocument patchedVenue, Guid id)
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

            return Ok();
        }
    }
}