using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ZipcodeShouldBePositiveException : ApplicationException
    {
        public ZipcodeShouldBePositiveException(string message) : base (message)
        {

        }
    }
}
