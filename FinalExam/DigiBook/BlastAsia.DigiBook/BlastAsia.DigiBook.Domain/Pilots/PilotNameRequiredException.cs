using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotNameRequiredException : ApplicationException
    {
        public PilotNameRequiredException(string message)
       : base(message)
        {

        }
    }
}