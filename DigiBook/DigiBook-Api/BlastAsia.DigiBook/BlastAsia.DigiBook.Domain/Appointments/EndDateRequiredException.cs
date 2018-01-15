using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class EndDateRequiredException
        :Exception
    {
        public EndDateRequiredException(string message)
            :base(message)
        {

        }
    }
}