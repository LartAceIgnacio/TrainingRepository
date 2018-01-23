using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DayTwoApp")]
    [Produces("application/json")]
    [Route("api/Pilots")]
    public class PilotController : Controller
    {
        private IPilotService pilotService;
        private IPilotRepository pilotRepository;

        public PilotController(IPilotService pilotService, IPilotRepository pilotRepository)
        {
            this.pilotService = pilotService;
            this.pilotRepository = pilotRepository;
        }

        [HttpGet, ActionName("GetPilots")]
        public IActionResult GetPilot(Guid? id)
        {
            var result = new List<Pilot>();
            if(id == null)
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
        public IActionResult CreatePilot([FromBody] Pilot pilot)
        {
            try
            {
                if(pilot == null)
                {
                    return BadRequest();
                }

                var result = this.pilotService.Save(Guid.Empty, pilot);

                return CreatedAtAction("GetPilot",
                    new { id = pilot.PilotId }, result
                    );
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpDelete]
        public object DeletePilot(Guid id)
        {
            var found = pilotRepository.Retrieve(id);

            if (found == null)
            {
                return NotFound();
            }

            this.pilotRepository.Delete(id);

            return NoContent();
        }

        [HttpPut]
        public object UpdatePilot([FromBody] Pilot pilot, Guid id)
        {
            if (pilot == null)
            {
                return BadRequest();
            }

            var oldPilot = this.pilotRepository.Retrieve(id);

            if (oldPilot == null)
            {
                return NotFound();
            }

            oldPilot.ApplyChanges(pilot);

            this.pilotService.Save(id, pilot);

            return Ok(pilot);
        }
    }
}