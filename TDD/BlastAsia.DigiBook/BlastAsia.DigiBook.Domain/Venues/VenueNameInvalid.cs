using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueNameInvalid
        : Exception
    {
        public VenueNameInvalid(string message)
            : base(message)
        {
                
        }
    }
}