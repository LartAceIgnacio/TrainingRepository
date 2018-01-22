using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Venues.Exceptions
{
    public class VenueNameException
        :Exception
    {
        public VenueNameException(String message)
            : base(message)
        {

        }
    }
}
