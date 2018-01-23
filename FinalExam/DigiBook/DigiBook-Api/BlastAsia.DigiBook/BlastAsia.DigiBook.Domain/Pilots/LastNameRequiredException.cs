using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class LastNameRequiredException
        : ApplicationException
    {
        public LastNameRequiredException(string message)
            :base(message)
        {

        }
    }
}