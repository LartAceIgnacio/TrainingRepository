using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models.Flights;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DemoApp")]
    [Produces("flight/json")]
    [Route("api/Flights")]
    public class FlightsController : Controller
    {
        private IFlightRepository flightRepository;
        private IFlightService flightService;
        private static List<Flight> flight = new List<Flight>();

        public FlightsController(IFlightRepository flightRepository
            , IFlightService flightService)
        {
            this.flightRepository = flightRepository;
            this.flightService = flightService;
        }
        [HttpGet, ActionName("GetFlights")]
        public IActionResult GetFlights(Guid? id)
        {
            var result = new List<Flight>();
            if(id == null)
            {
                result.AddRange(this.flightRepository.Retrieve());
            }
            else
            {
                var flight = this.flightRepository.Retrieve(id.Value);
                result.Add(flight);
            }
            return Ok();
        }
    }
}