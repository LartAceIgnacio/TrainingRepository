using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class TimeInclusiveException: Exception
    {
        public TimeInclusiveException(string message)
            :base(message)
        {

        }
    }
}