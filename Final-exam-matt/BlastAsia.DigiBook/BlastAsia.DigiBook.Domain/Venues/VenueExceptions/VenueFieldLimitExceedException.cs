using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Venues.VenueExceptions
{
    public class VenueFieldLimitExceedException : ApplicationException
    {
        public VenueFieldLimitExceedException(string message) : base(message)
        {

        }
    }
}
