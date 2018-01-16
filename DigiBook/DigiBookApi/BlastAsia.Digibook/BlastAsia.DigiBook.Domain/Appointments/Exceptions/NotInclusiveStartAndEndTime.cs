using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.Exceptions
{
    public class NotInclusiveStartAndEndTime : ApplicationException
    {
        public NotInclusiveStartAndEndTime(string message) : base(message)
        {

        }
    }
}
