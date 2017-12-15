using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.Exceptions
{
    public class EmployeeIdException : Exception
    {
        public EmployeeIdException(String message) : base(message)
        {

        }
    }
}
