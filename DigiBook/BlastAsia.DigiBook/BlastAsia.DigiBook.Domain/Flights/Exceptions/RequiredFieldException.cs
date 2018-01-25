using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Flights.Exceptions
{
    public class RequiredFieldException : Exception
    {
        public RequiredFieldException(string message)
            : base(message)
        {

        }
    }
}
