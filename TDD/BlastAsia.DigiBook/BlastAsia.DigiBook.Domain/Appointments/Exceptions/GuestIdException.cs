using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.Exceptions
{
    public class GuestIdException : Exception
    {
        public GuestIdException(String message) : base(message)
        {

        }
    }
}
