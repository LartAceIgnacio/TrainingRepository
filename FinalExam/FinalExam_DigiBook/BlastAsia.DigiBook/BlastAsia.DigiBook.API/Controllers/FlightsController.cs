﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Flights;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DemoApp")]
    [Produces("application/json")]
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
        [HttpGet, ActionName("GetFlightsWithPagination")]
        [Route("api/Flights/{page}/{record}")]
        public IActionResult GetFlightWithPagination(int page, int record, string filter)
        {
            var result = new Pagination<Flight>();
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
        [Route("api/Flights/{id}")]
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
        [Route("api/Flights/{id}")]
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
        [Route("api/Flights")]
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