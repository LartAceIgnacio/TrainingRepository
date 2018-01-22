using System;

namespace BlastAsia.DigiBook.Domain.Reservations
{
    public class VenueNameRequiredException: Exception
    {
        public VenueNameRequiredException(string message): base(message)
        {

        }
    }
}