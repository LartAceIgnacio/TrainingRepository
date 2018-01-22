using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Api.Utils;
using BlastAsia.DigiBook.Domain.Locations;
using BlastAsia.DigiBook.Domain.Models.Locations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Locations")]
    public class LocationController : Controller
    {
        private ILocationRepository locationRepository;
        private ILocationService locationService;

        public LocationController(ILocationRepository locationRepository, ILocationService locationService)
        {
            this.locationRepository = locationRepository;
            this.locationService = locationService;
        }

        [HttpGet]
        public IActionResult GetLocations(Guid? id)
        {
            var result = new List<Location>();
            if (id == null)
            {
                result.AddRange(this.locationRepository.Retreive());
            }
            else
            {
                var location = this.locationRepository.Retrieve(id.Value);
                result.Add(location);
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateLocations(
            [FromBody] Location location)
        {
            try
            {
                if (location == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = this.locationService.Save(Guid.Empty, location);

                    return CreatedAtAction("GetLocations",
                        new { id = location.LocationId }, result);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteLocations(Guid id)
        {
            try
            {
                var locToDelete = this.locationRepository.Retrieve(id);
                if (locToDelete == null)
                {
                    return NotFound();
                }
                else
                {
                    this.locationRepository.Delete(id);
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult UpdateLocations(
            [FromBody] Guid id, Location location)
        {
            try
            {
                if (location == null)
                {
                    return BadRequest();
                }
                var locToUpdate = this.locationRepository.Retrieve(id);
                if (locToUpdate == null)
                {
                    return NotFound();
                }

                locToUpdate.ApplyChanges(location);

                var result = this.locationService.Save(id, locToUpdate);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchLocations(
            [FromBody]Guid id, JsonPatchDocument patch)
        {
            try
            {
                if (patch == null)
                {
                    return BadRequest();
                }
                var location = this.locationRepository.Retrieve(id);
                if (location == null)
                {
                    return NotFound();
                }

                patch.ApplyTo(location);
                this.locationService.Save(id, location);
                return Ok(location);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}