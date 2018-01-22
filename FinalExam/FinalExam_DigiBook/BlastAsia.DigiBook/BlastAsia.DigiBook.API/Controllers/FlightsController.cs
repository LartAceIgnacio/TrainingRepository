using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models.Flights;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DemoApp")]
    [Produces("application/json")]
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
            if (id == null)
            {
                result.AddRange(this.flightRepository.Retrieve());
            }
            else
            {
                var flight = this.flightRepository.Retrieve(id.Value);
                result.Add(flight);
            }
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateFlight([FromBody] Flight flight)
        {
            try
            {
                if (flight == null)
                {
                    return BadRequest();
                }
                var result = this.flightService.Save(Guid.Empty, flight);

                return CreatedAtAction("GetFlights", new { id = flight.FlightId }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        public IActionResult DeleteFlight(Guid id)
        {
            var deletedFlight = flightRepository.Retrieve(id);
            if(deletedFlight == null)
            {
                return NotFound();
            }
            this.flightRepository.Delete(id);

            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateFlight([FromBody] Flight modifiedFlight, Guid id)
        {
            var flight = flightRepository.Retrieve(id);
            if(flight == null)
            {
                return BadRequest();
            }
            flight.ApplyChanges(modifiedFlight);
            flightService.Save(id, flight);
            return Ok();
        }
        [HttpPatch]
        public IActionResult PatchFlight(JsonPatchDocument patchedFlight, Guid id)
        {
            if(patchedFlight == null)
            {
                return BadRequest();
            }
            var flight = flightRepository.Retrieve(id);
            if(flight == null)
            {
                return NotFound();
            }

            patchedFlight.ApplyTo(flight);
            flightService.Save(id, flight);

            return Ok();
        }
    }
}