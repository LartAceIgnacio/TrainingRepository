using System;

namespace BlastAsia.DigiBook.Domain
{
    public class VenueDescriptionMaxLengthException: Exception
    {
        public VenueDescriptionMaxLengthException(string message)
            :base(message)
        {

        }
    }
}