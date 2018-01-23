using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Get()
        {

            var result = new List<Pilot>();
            result.AddRange(this.pilotRepository.Retrieve());
            return Ok(result);
        }

      
    }
}
