using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Cors;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("DemoAppDay2")]
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
            }
            else
            {
                result.Add(this.repo.Retrieve(id.Value));
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateContact([FromBody] Venue venue)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }

                var result = this.service.Save(Guid.Empty, venue);
                return CreatedAtAction("GetContact", new { venue.VenueId }, result);
            }
            catch (Exception)
            {

                return (BadRequest());
            }
        }

        [HttpDelete]
        public IActionResult DeleteContact(Guid id)
        {
            var venueToDelete = this.repo.Retrieve(id);
            if (venueToDelete == null)
            {
                return NotFound();
            }
            else
            {
                this.repo.Delete(id);
            }

            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateVenue([FromBody] Venue venue, Guid id)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }

                var venueToUpdate = this.repo.Retrieve(id);

                if (venueToUpdate == null)
                {
                    return NotFound();
                }

                venueToUpdate.ApplyChages(venue);

                var result = service.Save(id, venueToUpdate);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchVenue([FromBody] JsonPatchDocument patch, Guid id)
        {
            try
            {
                if (patch == null)
                {
                    return BadRequest();
                }

                var venueToPatch = repo.Retrieve(id);
                if (venueToPatch == null)
                {
                    return BadRequest();
                }
                patch.ApplyTo(venueToPatch);

                var result = service.Save(id, venueToPatch);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}