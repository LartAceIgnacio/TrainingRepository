using System;

namespace BlastAsia.DigiBook.Domain.Models.Flights
{
    public class Flight
    {
        public Guid FlightId { get; set; }
        public string CityOfOrigin { get; set; }
        public string CityOfDestination { get; set; }
        public DateTime ExpectedTimeOfArrivalDate { get; set; }
        public TimeSpan ExpectedTimeOfArrivalTime { get; set; }
        public DateTime ExpectedTimeOfDepartureDate { get; set; }
        public TimeSpan ExpectedTimeOfDepartureTime { get; set; }
        public string FlightCode { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

    }
}