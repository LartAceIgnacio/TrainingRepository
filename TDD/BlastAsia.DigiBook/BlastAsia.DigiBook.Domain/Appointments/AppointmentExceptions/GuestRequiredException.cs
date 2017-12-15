using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.AppointmentExceptions
{
    public class GuestRequiredException : ApplicationException
    {
        public GuestRequiredException(string message)  : base(message) { }
    }
}
