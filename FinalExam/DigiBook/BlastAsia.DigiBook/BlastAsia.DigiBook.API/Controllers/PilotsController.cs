using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("demoApp")]
    [Produces("application/json")]
    public class PilotsController : Controller
    {
        private IPilotService pilotService;
        private IPilotRepository pilotRepository;

        public PilotsController(IPilotService pilotService, IPilotRepository pilotRepository)
        {
            this.pilotService = pilotService;
            this.pilotRepository = pilotRepository;
        }

        [HttpGet, ActionName("GetPilotsWithPagination")]
        [Route("api/Pilots/{page}/{record}")]
        public IActionResult GetPilotsWithPagination(int page, int record, string filter)
        {
            var result = new Pagination<Pilot>();
            try
            {
                result = this.pilotRepository.Retrieve(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet, ActionName("GetPilot")]
        [Route("api/Pilot/{id?}")]
        
        public object GetPilots(Guid? id)
        {
            var result = new List<Pilot>();
            if (id == null)
            {
                result.AddRange(this.pilotRepository.Retrieve());
            }
            else
            {
                var pilot = this.pilotRepository.Retrieve(id.Value);
                result.Add(pilot);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("api/Pilot")]
        [Authorize]
        public IActionResult CreatePilot(
            [FromBody] Pilot pilot)
        {
            try
            {
                if (pilot == null)
                {
                    return BadRequest();
                }
                var result = this.pilotService.Save(Guid.Empty, pilot);

                return CreatedAtAction("GetPilot",
                    new { id = pilot.PilotId }, result);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }

        }

        [HttpDelete]
        [Route("api/Pilot/{id}")]
        [Authorize]
        public IActionResult DeletePilot(Guid id)
        {
            var pilotToDelete = this.pilotRepository.Retrieve(id);
            if (pilotToDelete != null)
            {
                this.pilotRepository.Delete(id);
                return NoContent();
            }
            return NotFound();
        }


        [HttpPut]
        [Route("api/Pilot/{id}")]
        [Authorize]
        public IActionResult UpdatePilot(
            [FromBody] Pilot pilot, Guid id)
        {
            try
            {
                if (pilot == null)
                {
                    return BadRequest();
                }

                var existingPilot = pilotRepository.Retrieve(id);
                if (existingPilot == null)
                {
                    return NotFound();
                }
                existingPilot.ApplyChanges(pilot);

                var result = this.pilotService.Save(id, pilot);

                return Ok(existingPilot);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("api/Pilot/{id}")]
        [Authorize]
        public IActionResult PatchPilot(
            [FromBody]JsonPatchDocument patchedPilot, Guid id)
        {
            try
            {
                if (patchedPilot == null)
                {
                    return BadRequest();
                }

                var pilot = pilotRepository.Retrieve(id);
                if (pilot == null)
                {
                    return NotFound();
                }

                patchedPilot.ApplyTo(pilot);
                pilotService.Save(id, pilot);

                return Ok(pilot);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
       
    }
}