using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Venue")]
    public class VenueController : Controller
    {
        private IVenueRepository repo;
        private IVenueService service;

        public VenueController(IVenueRepository repo, IVenueService service)
        {
            this.repo = repo;
            this.service = service;
        }

        [HttpGet, ActionName("GetContact")]
        public IActionResult GetContact(Guid? id)
        {
            var result = new List<Venue>();

            if (id == null)
            {
                result.AddRange(this.repo.Retrieve());
            } else
            {
                result.Add(this.repo.Retrieve(id.Value));
            }

            return Ok(result);
        }

        public IActionResult CreateContact([FromBody] Venue venue)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }
             
                var result = this.service.Save(Guid.Empty, venue);
                return CreatedAtAction("GetContact", new { venue.VenueId}, result);
            }
            catch (Exception)
            {

                return (BadRequest());
            }
        }

        public IActionResult DeleteContact(Guid id)
        {
            var venueToDelete = this.repo.Retrieve(id);
            if (venueToDelete == null) {
                return NotFound();
            } else
            {
                this.repo.Delete(id);
            }

            return NoContent();
        }
    }
}