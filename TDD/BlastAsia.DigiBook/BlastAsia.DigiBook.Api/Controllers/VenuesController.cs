using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Api.Utils;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Venues")]
    public class VenuesController : Controller
    {
        private readonly IVenueService venueService;
        private readonly IVenueRepository venueRepository;

        public VenuesController(IVenueService venueService, IVenueRepository venueRepository)
        {
            this.venueService = venueService;
            this.venueRepository = venueRepository;
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
        public IActionResult CreateVenue( [Bind] Venue venue)
        {
            try
            {
                var result = this.venueService.Save(Guid.Empty, venue);

                return CreatedAtAction("GetVenues", new { id = venue.VenueId }, result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        public IActionResult DeleteVenue(Guid id)
        {
            try
            {
                var deletedVenue = venueRepository.Retrieve(id);
                if(deletedVenue == null)
                {
                    return NotFound();
                }
                this.venueRepository.Delete(id);

                return Ok();

            }
            catch (Exception)
            {
                return BadRequest();
            }
           
        }

        [HttpPut]
        public IActionResult UpdateVenue(
            [FromBody] Venue modifiedVenue, Guid id)
        {
            try
            {
                var venue = venueRepository.Retrieve(id);
                venue.ApplyChanges(modifiedVenue);
                venueService.Save(id, venue);
                return Ok(venue);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPatch]
        public IActionResult PatchVenue([FromBody]JsonPatchDocument patchedVenue, Guid id)
        {
            try
            {
                var venue = venueRepository.Retrieve(id);
                patchedVenue.ApplyTo(venue);
                venueService.Save(id, venue);

                return Ok(venue);
            }
            catch(Exception)
            {
                return NotFound();
            }
        }
    }
}