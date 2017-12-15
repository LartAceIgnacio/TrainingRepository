using System;

namespace BlastAsia.Digibook.Domain.Appointments
{
    public class InvalidTimeRangeException:Exception
    {
        public InvalidTimeRangeException(string message) : base(message)
        {

        }
    }
}