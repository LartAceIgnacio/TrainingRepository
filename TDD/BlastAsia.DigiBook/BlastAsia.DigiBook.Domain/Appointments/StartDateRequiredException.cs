using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class StartDateRequiredException
        :Exception

    {
        public StartDateRequiredException(string message)
            :base(message)
        {
                
        }
    }
}