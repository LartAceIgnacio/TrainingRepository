using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.AppointmentExceptions
{
    public class InvalidTimeScheduleException : ApplicationException
    {
        public InvalidTimeScheduleException(string message) : base(message) { }
    }
}
