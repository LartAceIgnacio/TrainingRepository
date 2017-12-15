using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentDateLapsedAlreadyException
        :Exception
    {
        public AppointmentDateLapsedAlreadyException(string message)
            : base(message)
        {

        }
    }
}