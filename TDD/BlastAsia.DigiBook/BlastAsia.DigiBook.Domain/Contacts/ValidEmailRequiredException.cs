using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ValidEmailRequiredException : Exception
    {
        public ValidEmailRequiredException(String message) : base(message)
        {

        }
    }
}
