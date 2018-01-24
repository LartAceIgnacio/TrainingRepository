using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.Adapters
{
    public interface IDateTimeWrapper
    {
        DateTime GetNow();

        DateTime GetDate();
        //DateTime GetTime();

        TimeSpan GetTime();
    }
}
