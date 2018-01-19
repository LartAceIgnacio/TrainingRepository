using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class NonExistingContactException
        :Exception
    {
        public NonExistingContactException(string message)
            : base(message)
        {

        }
    }
}