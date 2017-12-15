using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Employees.Exceptions
{
    public class EmailAddressRequiredException :Exception
    {
        public EmailAddressRequiredException(String message) : base(message)
        {

        }
    }
}
