using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class ValidAppointmentDateRequiredException 
        : Exception
    {
        public ValidAppointmentDateRequiredException(string message)
            : base(message)
        {

        }
    }
}