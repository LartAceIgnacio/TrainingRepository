using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("DemoAppDay2")]
    [Produces("application/json")]
    //[Route("api/Pilot")]
    public class PilotsController : Controller
    {
        private static List<Pilot> pilots = new List<Pilot>();

        private readonly IPilotRepository pilotRepository;
        private readonly IPilotService pilotService;
        public PilotsController(IPilotRepository pilotRepository, IPilotService pilotService)
        {
            this.pilotRepository = pilotRepository;
            this.pilotService = pilotService;
        }

        [HttpGet, ActionName("GetPilots")]
        [Route("api/Pilot")]
        public IActionResult GetPilot(Guid? id)
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
        [Authorize]
        [Route("api/Pilot")]
        public IActionResult CreatePilot([FromBody] Pilot pilot)
        {
            try
            {
                if (pilot == null)
                {
                    return BadRequest();
                }

                var result = this.pilotService.Save(pilot.PilotId, pilot);
                return CreatedAtAction("GetPilots", new { id = pilot.PilotId }, result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/Pilot/{pageNumber}/{recordNumber}/")]
        public IActionResult GetPilot(int pageNumber, int recordNumber, string query)
        {
            try
            {
                var result = new Pagination<Pilot>();
                result = this.pilotRepository.Retrieve(pageNumber, recordNumber, query);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("api/Pilot")]
        public IActionResult DeletePilot(Guid id)
        {
            var pilotToDelete = pilotRepository.Retrieve(id);

            if (pilotToDelete == null)
            {
                return NotFound();
            }

            pilotRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Authorize]
        [Route("api/Pilot")]
        public IActionResult UpdatePilot(
            [FromBody]
                Pilot pilot,
                    Guid id
            )
        {
            try
            {

                if (pilot == null)
                {
                    return BadRequest();
                }

                var pilotToUpdate = pilotRepository.Retrieve(id);

                if (pilotToUpdate == null)
                {
                    return NotFound();
                }

                pilotToUpdate.ApplyChanges(pilot);

                var result = pilotService.Save(id, pilotToUpdate);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPatch]
        [Authorize]
        [Route("api/Pilot")]
        public IActionResult PatchPilot([FromBody]JsonPatchDocument patchedPilot, Guid id)
        {


            try
            {

                if (patchedPilot == null)
                {
                    return BadRequest();
                }

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
