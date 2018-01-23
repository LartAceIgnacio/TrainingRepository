using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class RequiredException
        :Exception
    {
        public RequiredException(string message)
            : base(message)
        {

        }
    }
}