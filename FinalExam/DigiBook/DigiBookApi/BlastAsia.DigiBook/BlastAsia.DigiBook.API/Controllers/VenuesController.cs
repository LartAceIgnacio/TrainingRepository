using System;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Models.Venues;
using Microsoft.AspNetCore.Mvc;
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
        private static List<Venue> venues = new List<Venue>();
        private IVenueRepository venueRepository;
        private IVenueService venueService;

        public VenuesController(IVenueRepository venueRepository, IVenueService venueService)
        {
            this.venueRepository = venueRepository;
            this.venueService = venueService;
        }

        [HttpGet, ActionName("GetVenuesWithPagination")]
        [Route("api/Venues/{page}/{record}")]
        public IActionResult GetVenuesWithPagination(int page, int record, string filter)
        { 
             var result = new PaginationClass<Venue>();
            try {
                result = this.venueRepository.Retrieve(page, record, filter);
            }
            catch (Exception) {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet, ActionName("GetVenues")]
        [Route("api/Venues/{id?}")]
        public IActionResult GetVenues(Guid? Id)
        {
            List<Venue> result = new List<Venue>();

            if (Id == null)
            {
                result.AddRange(this.venueRepository.Retrieve());
            }
            else
            {
                var found = this.venueRepository.Retrieve(Id.Value);
                result.Add(found);
            }

            return Ok(result);
        }
        
        [HttpPost]
        [Route("api/Venues")]
        [Authorize]
        public IActionResult CreateVenue([FromBody]Venue venue)
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
        [Route("api/Venues/{id}")]
        [Authorize]
        public IActionResult DeleteVenue(Guid id)
        {
            var venueToDelete = this.venueRepository.Retrieve(id);
            if (venueToDelete == null)
            {
                return NotFound();
            }

            venueRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Route("api/Venues/{id}")]
        [Authorize]
        public IActionResult UpdateVenue([FromBody] Venue venue, Guid id)
        {
            if (venue == null)
            {
                return BadRequest();
            }

            var venueToUpdate = venueRepository.Retrieve(id);
            if (venueToUpdate == null)
            {
                return NotFound();
            }
            venueToUpdate.ApplyChanges(venue);
            this.venueRepository.Update(id, venueToUpdate);
            return Ok(venue);
        }

        [HttpPatch]
        [Route("api/Venues/{id}")]
        [Authorize]
        public IActionResult PatchVenue([FromBody]JsonPatchDocument patchedVenue, Guid id)
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