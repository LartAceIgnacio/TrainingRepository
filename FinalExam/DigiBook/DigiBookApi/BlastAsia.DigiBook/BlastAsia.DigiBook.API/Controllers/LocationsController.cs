using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Locations;
using BlastAsia.DigiBook.Domain.Models.Locations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DayTwoApp")]
    [Produces("application/json")]
    [Route("api/Locations")]
    public class LocationsController : Controller
    {
        private ILocationRepository locationRepository;
        private ILocationService locationService;

        public LocationsController(ILocationRepository locationRepository, ILocationService locationService)
        {
            this.locationRepository = locationRepository;
            this.locationService = locationService;
        }

        [HttpGet, ActionName("GetLocations")]
        public IActionResult GetLocations(Guid? id)
        {
            List<Location> result = new List<Location>();

            if (id == null)
            {
                result.AddRange(this.locationRepository.Retrieve());
            }
            else
            {
                var found = this.locationRepository.Retrieve(id.Value);
                result.Add(found);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult PostLocation([FromBody]Location location)
        {
            try
            {
                if (location == null)
                {
                    return BadRequest();
                }

                var result = this.locationService.Save(Guid.Empty, location);

                return CreatedAtAction("GetLocations",
                    new { id = location.LocationId }, result);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteLocation(Guid id)
        {
            var locationToDelete = this.locationRepository.Retrieve(id);
            if (locationToDelete == null)
            {
                return NotFound();
            }

            locationRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult PutLocation([FromBody] Location location, Guid id)
        {
            if (location == null)
            {
                return BadRequest();
            }

            var locationToUpdate = locationRepository.Retrieve(id);
            if (locationToUpdate == null)
            {
                return NotFound();
            }
            locationToUpdate.ApplyChanges(location);
            this.locationRepository.Update(id, locationToUpdate);
            return Ok(location);
        }

        [HttpPatch]
        public IActionResult PatchLocation([FromBody]JsonPatchDocument patchedLocation, Guid id)
        {
            if (patchedLocation == null)
            {
                return BadRequest();
            }

            var location = locationRepository.Retrieve(id);
            if (location == null)
            {
                return NotFound();
            }

            patchedLocation.ApplyTo(location);
            locationService.Save(id, location);

            return Ok(location);
        }
    }
}