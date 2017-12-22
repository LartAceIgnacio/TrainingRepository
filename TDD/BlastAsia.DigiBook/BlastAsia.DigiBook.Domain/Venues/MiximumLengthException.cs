using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class MaximumLengthException: Exception
    {
        public MaximumLengthException(string message)
            : base (message)
        {

        }
    }
}