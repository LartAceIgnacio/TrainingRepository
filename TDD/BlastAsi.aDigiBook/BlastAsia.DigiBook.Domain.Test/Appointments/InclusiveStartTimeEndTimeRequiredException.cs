using System;

namespace BlastAsia.DigiBook.Domain.Test.Appointments
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