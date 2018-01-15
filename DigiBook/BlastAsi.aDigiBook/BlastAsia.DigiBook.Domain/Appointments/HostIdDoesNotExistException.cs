using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class HostIdDoesNotExistException
    : Exception
    {
        public HostIdDoesNotExistException(string message)
            :base(message)
        {

        }
    }
}