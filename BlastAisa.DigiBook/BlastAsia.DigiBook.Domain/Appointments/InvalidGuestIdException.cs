using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InvalidGuestIdException : ApplicationException
    {
        public InvalidGuestIdException(string message) : base (message)
        {

        }
    }
}
