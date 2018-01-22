using System;

namespace BlastAsia.DigiBook.Domain.Reservations
{
    public class ScheduleRequiredException: Exception
    {
        public ScheduleRequiredException(string message): base(message)
        {

        }
    }
}