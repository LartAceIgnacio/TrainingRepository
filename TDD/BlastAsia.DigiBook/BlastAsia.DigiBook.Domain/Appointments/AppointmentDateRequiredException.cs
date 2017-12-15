using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentDateRequiredException
        :Exception
    {
        public AppointmentDateRequiredException(string message)
            :base(message)
        {

        }
    }
}