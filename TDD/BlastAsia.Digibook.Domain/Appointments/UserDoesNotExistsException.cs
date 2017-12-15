using System;

namespace BlastAsia.Digibook.Domain.Appointments
{
    public class UserDoesNotExistsException:Exception
    {
        public UserDoesNotExistsException(string message) : base(message)
        {

        }
    }
}