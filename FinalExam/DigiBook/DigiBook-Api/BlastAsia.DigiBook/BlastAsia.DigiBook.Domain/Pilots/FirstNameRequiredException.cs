using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class FirstNameRequiredException
        :ApplicationException
    {
        public FirstNameRequiredException(string message)
            :base(message)
        {

        }
    }
}