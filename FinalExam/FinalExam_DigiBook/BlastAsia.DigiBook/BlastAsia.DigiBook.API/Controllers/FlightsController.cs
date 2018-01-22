using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Flights;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    public class FlightsController : Controller
    {
        private IFlightRepository flightRepository;
        private IFlightService flightService;

        public FlightsController(IFlightRepository flightRepository
            , IFlightService flightService)
        {
            this.flightRepository = flightRepository;
            this.flightService = flightService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public object GetFlights(object p)
        {
            throw new NotImplementedException();
        }
    }
}