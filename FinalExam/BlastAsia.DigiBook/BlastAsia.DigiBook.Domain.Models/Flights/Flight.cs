using System;

namespace BlastAsia.DigiBook.Domain.Models.Flights
{
    public class Flight
    {
        public string CityOfOrigin { get; set; }
        public string CityOfDestination { get; set; }
        public DateTime Eta { get; set; }
        public DateTime Etd { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string FlightCode { get; set; }
        public Guid FlightId { get; set; }
    }
}