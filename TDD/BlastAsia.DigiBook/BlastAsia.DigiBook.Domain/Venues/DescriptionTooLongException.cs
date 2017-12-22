using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class DescriptionTooLongException
        : Exception
    {
        public DescriptionTooLongException(string message)
            : base(message)
        {

        }
    }
}