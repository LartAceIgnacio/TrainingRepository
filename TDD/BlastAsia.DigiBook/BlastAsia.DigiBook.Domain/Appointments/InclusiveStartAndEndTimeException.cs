using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InclusiveStartAndEndTimeException
        :Exception
    {
        public InclusiveStartAndEndTimeException(string message)
            :base (message)
        {

        }
    }
}