using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.Exceptions
{
    public class TimeInclusiveException : Exception
    {
        public TimeInclusiveException(String message) : base(message)
        {

        }
    }
}
