using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class PhotoRequiredException : ApplicationException
    {
        public PhotoRequiredException(string message) : base(message)
        {

        }
    }
}
