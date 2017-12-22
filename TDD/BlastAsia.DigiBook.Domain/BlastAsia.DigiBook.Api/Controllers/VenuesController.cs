using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Api.Controllers
{
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
            var result = new List<Venue>();
            if(id == null)
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

        public IActionResult CreateContact([FromBody] Venue venue)
        {
            this.venueService.Save(Guid.Empty, venue);
            return CreatedAtAction("GetVenues", new { id = venue.VenueId }, venue);
        }
    }
}