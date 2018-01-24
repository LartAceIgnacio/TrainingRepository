using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.AppointmentExceptions
{
    public class HostRequiredException : ApplicationException
    {
        public HostRequiredException(string message) : base(message) { }
    }
}
