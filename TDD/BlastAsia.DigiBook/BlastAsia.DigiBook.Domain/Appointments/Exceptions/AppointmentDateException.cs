using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.Exceptions
{
    public class AppointmentDateException : Exception
    {
        public AppointmentDateException(String message) : base(message)
        {

        }
    }
}
