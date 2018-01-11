using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class EmployeeIdNotExistedException : Exception
    {
        public EmployeeIdNotExistedException(string message) 
            : base(message)
        {

        }
    }
}