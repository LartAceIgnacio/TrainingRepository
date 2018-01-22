using System;

namespace BlastAsia.DigiBook.Domain.Locations
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