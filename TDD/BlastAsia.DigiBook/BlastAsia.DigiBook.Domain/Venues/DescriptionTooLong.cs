using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class DescriptionTooLong
        : Exception
    {
        public DescriptionTooLong(string message)
            : base(message)
        {

        }
    }
}