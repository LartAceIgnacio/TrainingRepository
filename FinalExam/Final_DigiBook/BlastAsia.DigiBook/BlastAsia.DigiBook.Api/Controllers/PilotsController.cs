using System;

using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Models.Records;
using BlastAsia.DigiBook.Domain.Pilots;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Pilots")]
    public class PilotsController : Controller
    {
        private readonly IPilotRepository pilotRepository;
        private readonly IPilotService pilotService;
        public PilotsController(IPilotService pilotService, IPilotRepository pilotRepository ){
            this.pilotRepository = pilotRepository;
            this.pilotService = pilotService;
        }
        // GET: api/Pilots
        [HttpGet]
        public IActionResult GetPilotsWithPagination(int page, int record, string filter)
        {
            var result = new Record<Pilot>();
            try
            {
                result = this.pilotRepository.Fetch(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeletePilot(Guid id)
        {
            var deletedPilot = pilotRepository.Retrieve(id);
            if (deletedPilot == null)
            {
                return NotFound();
            }

            this.pilotRepository.Delete(id);

            return Ok();
        }
    }
}
