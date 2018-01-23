using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class NonExistingEmployeeException
        :Exception
    {
       public NonExistingEmployeeException(string message)
            : base(message)
        {

        }
    }
}