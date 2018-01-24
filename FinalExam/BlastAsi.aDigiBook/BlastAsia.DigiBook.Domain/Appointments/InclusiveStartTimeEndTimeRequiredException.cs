using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InclusiveStartTimeEndTimeRequiredException 
        : Exception
    {
        public InclusiveStartTimeEndTimeRequiredException(string message)
            :base(message)
        {

        }
    }
}