using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InvalidTimeException : ApplicationException
    {
        public InvalidTimeException(string message) : base (message)
        {

        }
    }
}
