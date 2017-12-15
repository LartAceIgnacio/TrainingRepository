using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InvalidAppointmentDateException : ApplicationException
    {
        public InvalidAppointmentDateException(String message) : base (message)
        {

        }
    }
}
