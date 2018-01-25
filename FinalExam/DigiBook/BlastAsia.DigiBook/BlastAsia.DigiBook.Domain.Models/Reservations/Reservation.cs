using System;

namespace BlastAsia.DigiBook.Domain.Models.Reservations
{
    public class Reservation
    {
        public string VenueName { get; set; }
        public string Description { get; set; }
        public Guid ReservationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}