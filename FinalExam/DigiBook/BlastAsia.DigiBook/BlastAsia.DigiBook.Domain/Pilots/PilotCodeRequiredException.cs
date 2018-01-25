using System;
using System.Runtime.Serialization;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotCodeRequiredException : ApplicationException
    {
   
        public PilotCodeRequiredException(string message) : base(message)
        {
        }

        
    }
}