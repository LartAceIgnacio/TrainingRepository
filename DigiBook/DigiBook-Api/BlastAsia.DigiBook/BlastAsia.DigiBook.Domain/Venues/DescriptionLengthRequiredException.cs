using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class DescriptionLengthRequiredException
        :Exception
    {
        public DescriptionLengthRequiredException(string message)
            :base(message)
        {

        }
    }
}