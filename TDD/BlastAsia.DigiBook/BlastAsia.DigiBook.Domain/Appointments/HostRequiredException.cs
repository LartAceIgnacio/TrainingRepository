using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class HostRequiredException
        :Exception
    {
        public HostRequiredException(string message)
            :base(message)
        {

        }
    }
}