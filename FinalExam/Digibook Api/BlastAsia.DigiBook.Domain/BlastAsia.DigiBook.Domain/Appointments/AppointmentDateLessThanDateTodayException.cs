using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentDateLessThanDateTodayException: Exception
    {
        public AppointmentDateLessThanDateTodayException(string message)
            :base(message)
        {

        }
    }
}