using System;

namespace BlastAsia.DigiBook.Domain.Appointments.Exceptions
{
    public class AppointmentDateException : Exception
    {
        public AppointmentDateException(string message) : base(message)
        {
        }
    }
}