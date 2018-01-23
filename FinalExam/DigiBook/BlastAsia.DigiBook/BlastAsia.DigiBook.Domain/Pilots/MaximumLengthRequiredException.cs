using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class MaximumLengthRequiredException : ApplicationException
    {
        public MaximumLengthRequiredException(string message)
      : base(message)
        {

        }
    }
}