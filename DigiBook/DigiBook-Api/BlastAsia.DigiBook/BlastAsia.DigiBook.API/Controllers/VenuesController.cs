using System;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Venues;
using System.Collections.Generic;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DayTwoApp")]
    [Produces("application/json")]
    //[Route("api/Venues")]
    public class VenuesController : Controller
    {
        private readonly IVenueService venueService;
        private readonly IVenueRepository venueRepository;

        public VenuesController(IVenueService venueService, IVenueRepository venueRepository)
        {
            this.venueService = venueService;
            this.venueRepository = venueRepository;
        }



        [HttpGet, ActionName("GetVenuesWithPagination")]
        [Route("api/Venues/{page}/{record}")]
        public IActionResult GetVenuesWithPagination(int page, int record, string filter)
        {
            var result = new PaginationResult<Venue>();
            try
            {
                result = this.venueRepository.Retrieve(page, record, filter);
            }
            catch (Exception ex)
            {
                throw ex;
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

                result.AddRange(this.venueRepository.Retrieve());
            }
            else
            {
                var contact = this.venueRepository.Retrieve(id.Value);
                result.Add(contact);
            }


            return Ok(result);
        }

        [HttpPost]
        [Route("api/Venues")]
        [Authorize]
        public IActionResult CreateVenue(
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
                    new { id = venue.VenueId}, result);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpDelete]
        [Route("api/Venues/{id}")]
        [Authorize]
        public IActionResult DeleteVenue(Guid id)
        {
            var foundDeleteId = venueRepository.Retrieve(id);
            if (foundDeleteId == null)
            {
                return NotFound();
            }

            this.venueRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Route("api/Venues/{id}")]
        [Authorize]
        public IActionResult UpdateVenue(
            [FromBody] Venue venue, Guid id)
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

            this.venueService.Save(id, venue);

            return Ok(venue);
        }

        [HttpPatch]
        [Route("api/Venues/{id}")]
        [Authorize]
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