using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class UniquePilotCodeRequiredException
        :ApplicationException
    {
        public UniquePilotCodeRequiredException(string message)
            : base(message)
        {

        }
    }
}