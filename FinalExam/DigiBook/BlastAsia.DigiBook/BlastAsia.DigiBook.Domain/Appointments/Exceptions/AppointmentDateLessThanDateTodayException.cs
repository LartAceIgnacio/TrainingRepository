using System;

namespace BlastAsia.DigiBook.Domain.Appointments.Exceptions
{
    public class AppointmentDateLessThanDateTodayException : Exception
    {
        public AppointmentDateLessThanDateTodayException(string message) : base(message)
        {
        }
    }
}