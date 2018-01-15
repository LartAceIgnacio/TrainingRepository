using System;

namespace BlastAsia.DigiBook.Domain.Appointments.Exceptions
{
    public class EmployeeIdRequiredException : Exception
    {
        public EmployeeIdRequiredException(string message) : base(message)
        {
        }
    }
}