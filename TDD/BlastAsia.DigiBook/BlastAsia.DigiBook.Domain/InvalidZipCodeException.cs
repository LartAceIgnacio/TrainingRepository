using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public class InvalidZipCodeException : ApplicationException
    {

        public InvalidZipCodeException(string message) : base(message){ }
    }
}
