using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Flights;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
    [Produces("application/json")]
    public class FlightsController : Controller
    {
        private IFlightService flightService;
        private IFlightRepository flightRepository;

        public FlightsController(IFlightService flightService, IFlightRepository flightRepository)
        {
            this.flightService = flightService;
            this.flightRepository = flightRepository;
        }

        [HttpGet, ActionName("GetFlightsWithPagination")]
        [Route("api/Flights/{page}/{record}")]
        public IActionResult GetFlightsWithPagination(int page, int record, string filter)
        {
            var result = new PaginationResult<Flight>();
            try
            {
                result = this.flightRepository.Retrieve(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet, ActionName("GetFlightsWithFlightCode")]
        [Route("api/Flights/{code}")]
        public IActionResult GetFlightsWithFlightCode(string code)
        {
            var result = new Flight();
            try
            {
                result = this.flightRepository.Retrieve(code);
            }
            catch (Exception)
            {

                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet, ActionName("GetFlights")]
        [Route("api/Flights")]
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
        [Route("api/Flights")]
        public IActionResult CreateFlight([FromBody]Flight flight)
        {
            try
            {
                if (flight == null)
                {
                    return BadRequest();
                }
                var result = this.flightService.Save(Guid.Empty, flight);
                return CreatedAtAction("GetFlights", new { id = flight.FlightId }, flight);
            }
            catch (Exception)
            {
                return BadRequest();
            }    
        }

        [HttpDelete]
        [Route("api/Flights/{id}")]
        public IActionResult DeleteFlight(Guid id)
        {
            var flightToDelete = this.flightRepository.Retrieve(id);
            if (flightToDelete == null)
            {
                return NotFound();
            }
            this.flightRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Route("api/Flights/{id}")]
        public IActionResult UpdateFlight([FromBody]Flight flight, Guid id)
        {
            try
            {
                if (flight == null)
                {
                    return BadRequest();
                }
                var existingFlight = flightRepository.Retrieve(id);
                if (existingFlight == null)
                {
                    return NotFound();
                }
                existingFlight.ApplyChanges(flight);
                var result = this.flightService.Save(id, existingFlight);
                return Ok(flight);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("api/Flights/{id}")]
        public IActionResult PatchFlight([FromBody] JsonPatchDocument patchedFlight, Guid id)
        {
            if (patchedFlight == null)
            {
                return BadRequest();
            }
            var flight = flightRepository.Retrieve(id);
            if (flight == null)
            {
                return NotFound();
            }
            patchedFlight.ApplyTo(flight);
            flightService.Save(id, flight);
            return Ok(flight);
        }
    }
}