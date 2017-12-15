using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InclusiveTimeEndTimeLessThanStartTimeException
            :Exception
        {
        public InclusiveTimeEndTimeLessThanStartTimeException(string message)
            : base(message)
        {

        }
    }
}