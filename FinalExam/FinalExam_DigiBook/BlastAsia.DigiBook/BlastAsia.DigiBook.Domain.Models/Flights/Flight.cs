using System;

namespace BlastAsia.DigiBook.Domain.Models.Flights
{
    public class Flight
    {
        public Guid FlightId { get; set; }
        public string CityOfOrigin { get; set; }
        public string CityOfDestination { get; set; }
        public DateTime? ExpectedTimeOfArrival { get; set; }
        public DateTime? ExpectedTimeOfDeparture { get; set; }
        public string FlightCode { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        
    }
}