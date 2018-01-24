using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class ValidPilotCodeRequiredException
        : ApplicationException
    {
        public ValidPilotCodeRequiredException(string message)
            :base(message)
        {

        }
    }
}