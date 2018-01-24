using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Venues.VenueExceptions
{
    public class VenueNameRequiredException : ApplicationException
    {
        public VenueNameRequiredException(string message) : base(message)
        {
        }
    }
}
