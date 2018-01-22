using System;

namespace BlastAsia.DigiBook.Domain.Locations
{
    public class LocationNameRequiredException
        : Exception
    {
        public LocationNameRequiredException(string message)
            : base(message)
        {

        }
    }
}