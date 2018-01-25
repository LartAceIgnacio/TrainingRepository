using BlastAsia.DigiBook.Domain.Models.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class ReservationExtensions
    {
        public static Reservation ApplyChanges(this Reservation reservation,
            Reservation from)
        {
            reservation.VenueName = from.VenueName;
            reservation.Description = from.Description;
            reservation.StartDate = from.StartDate;
            reservation.EndDate = from.EndDate;

            return reservation;
        }
    }
}
