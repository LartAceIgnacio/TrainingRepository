using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.Adapters
{
    public class DateTimeWrapper : IDateTimeWrapper
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }

        public DateTime GetDate()
        {
            return DateTime.Now.Date;
        }

        public DateTime GetTime()
        {
            return DateTime.Now.ToLocalTime();
        }
    }
}
