using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DayTwoApp")]
    [Produces("application/json")]
    // [Route("api/Pilots")]
    public class PilotController : Controller
    {
        private IPilotService pilotService;
        private IPilotRepository pilotRepository;
        private string pilotCodeName;
        private string pilotCodeYear;

        public PilotController(IPilotService pilotService, IPilotRepository pilotRepository)
        {
            this.pilotService = pilotService;
            this.pilotRepository = pilotRepository;
        }

        [HttpGet, ActionName("GetPilotsWithPagination")]
        [Route("api/Pilots/{page}/{record}")]
        public IActionResult GetPilotsWithPagination(int page, int record, string filter)
        {
            //   var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = new PaginationResult<Pilot>();
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
        [Route("api/Pilots/{id?}")]
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
        [Route("api/Pilots")]
        [Authorize]
        public IActionResult CreatePilot([FromBody] Pilot pilot)
            {
            try
            {
                if(pilot == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(pilot.MiddleName))
                    {
                        pilotCodeName = pilot.FirstName.Substring(0, 2).ToUpper() + pilot.LastName.Substring(0, 4).ToUpper();
                        pilotCodeYear = pilot.DateActivated.Value.ToString("yy") +
                            pilot.DateActivated.Value.Month.ToString().PadLeft(2, '0') +
                            pilot.DateActivated.Value.Day.ToString().PadLeft(2, '0');

                        pilot.PilotCode = pilotCodeName + pilotCodeYear;
                    }
                    else
                    {
                        pilotCodeName = pilot.FirstName.Substring(0, 2).ToUpper() +
                            pilot.MiddleName.Substring(0, 2).ToUpper() +
                            pilot.LastName.Substring(0, 4).ToUpper();
                        pilotCodeYear = pilot.DateActivated.Value.ToString("yy") +
                            pilot.DateActivated.Value.Month.ToString().PadLeft(2, '0') +
                            pilot.DateActivated.Value.Day.ToString().PadLeft(2, '0');

                        pilot.PilotCode = pilotCodeName + pilotCodeYear;

                    }

                    pilot.DateCreated = DateTime.Now;
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
        [Route("api/Pilots/{id}")]
        [Authorize]
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
        [Route("api/Pilots/{id}")]
        [Authorize]
        public object UpdatePilot([FromBody] Pilot pilot, Guid id)
        {
            if (pilot == null)
            {
                return BadRequest();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(pilot.MiddleName))
                {
                    pilotCodeName = pilot.FirstName.Substring(0, 2).ToUpper() + pilot.LastName.Substring(0, 4).ToUpper();
                    pilotCodeYear = pilot.DateActivated.Value.ToString("yy") +
                        pilot.DateActivated.Value.Month.ToString().PadLeft(2, '0') +
                        pilot.DateActivated.Value.Day.ToString().PadLeft(2, '0');

                    pilot.PilotCode = pilotCodeName + pilotCodeYear;
                }
                else
                {
                    pilotCodeName = pilot.FirstName.Substring(0, 2).ToUpper() +
                        pilot.MiddleName.Substring(0, 2).ToUpper() +
                        pilot.LastName.Substring(0, 4).ToUpper();
                    pilotCodeYear = pilot.DateActivated.Value.ToString("yy") +
                        pilot.DateActivated.Value.Month.ToString().PadLeft(2, '0') +
                        pilot.DateActivated.Value.Day.ToString().PadLeft(2, '0');

                    pilot.PilotCode = pilotCodeName + pilotCodeYear;

                }

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