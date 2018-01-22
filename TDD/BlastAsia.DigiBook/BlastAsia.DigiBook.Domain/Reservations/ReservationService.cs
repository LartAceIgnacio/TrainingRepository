using System;
using BlastAsia.DigiBook.Domain.Models.Reservations;

namespace BlastAsia.DigiBook.Domain.Reservations
{
    public class ReservationService: IReservationService
    {
        private IReservationRepository reservationRepository;
        private readonly int venueNameMaxCharacter = 50;
        private readonly int descriptionMaxCharacter = 100;

        public ReservationService(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }

        public Reservation Save(Guid id, Reservation reservation)
        {
            if (String.IsNullOrEmpty(reservation.VenueName))
            {
                throw new VenueNameRequiredException("Venue Name is Required.");
            }
            if (reservation.VenueName.Length > venueNameMaxCharacter)
            {
                throw new MaximumCharacterExceededException("Max length is 50 characters.");
            }
            if (reservation.Description.Length > descriptionMaxCharacter)
            {
                throw new MaximumCharacterExceededException("Max Length is 100 characters.");
            }
            if (reservation.StartDate < DateTime.Now)
            {
                throw new ScheduleRequiredException("Start Date should not be less than the current date");
            }
            if (reservation.StartDate == reservation.EndDate)
            {
                throw new ScheduleRequiredException("Start date and End date should not be the same date");
            }
            if (reservation.EndDate <= DateTime.Now)
            {
                throw new ScheduleRequiredException("End Date should not be current date.");
            }

            Reservation result = null;
            var found = reservationRepository.Retrieve(id);

            if (found == null)
            {
                result = reservationRepository.Create(reservation);
            }
            else
            {
                result = reservationRepository.Update(id, reservation);
            }

            return result;
        }

    }
}