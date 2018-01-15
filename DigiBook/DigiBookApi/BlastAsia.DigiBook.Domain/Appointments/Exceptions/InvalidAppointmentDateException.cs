using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.Exceptions
{
    public class InvalidAppointmentDateException : ApplicationException
    {
        public InvalidAppointmentDateException(string message) : base(message)
        {

        }
    }
}
