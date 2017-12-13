using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public class InvalidEmailFormatException : ApplicationException
    {
        public InvalidEmailFormatException(string message) : base(message) { }
    }
}
