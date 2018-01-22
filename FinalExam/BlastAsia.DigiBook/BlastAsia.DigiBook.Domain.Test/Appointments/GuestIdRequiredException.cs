using System;

namespace BlastAsia.DigiBook.Domain.Test.Appointments
{
    public class GuestIdRequiredException
        :Exception
    {
        public GuestIdRequiredException(string message)
            :base(message)
        {

        }
    }
}