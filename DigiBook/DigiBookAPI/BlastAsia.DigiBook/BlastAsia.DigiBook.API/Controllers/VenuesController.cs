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
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DemoApp")]
    [Produces("application/json")]
    //[Route("api/Venues")]
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

        [HttpGet, ActionName("GetVenuesWithPagination")]
        [Route("api/Venues/{page}/{record}")]
        public IActionResult GetVenuesWithPagination(int page, int record, string filter)
        {
            var result = new Pagination<Venue>();
            try
            {
                result = this.venueRepository.Retrieve(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet, ActionName("GetVenues")]
        [Route("api/Venues")]
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
        [Route("api/Venues")]
        public IActionResult CreateVenue(
            [FromBody] Venue venue)
        {
            try
            {
                if(venue == null)
                {
                    return BadRequest();
                }
                var result = this.venueService
                    .Save(Guid.Empty, venue);

                return CreatedAtAction("GetVenues", new { id = venue.VenueId }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/Venues")]
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
            return Ok(venue);
        }

        [HttpDelete]
        [Route("api/Venues")]
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
        [Route("api/Venues")]
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

            return Ok(venue);
        }
    }
}