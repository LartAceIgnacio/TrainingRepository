using BlastAsia.DigiBook.Domain.Models.Reservations;
using System;

namespace BlastAsia.DigiBook.Domain.Reservations
{
    public interface IReservationService
    {
        Reservation Save(Guid id, Reservation reservation);
    }
}