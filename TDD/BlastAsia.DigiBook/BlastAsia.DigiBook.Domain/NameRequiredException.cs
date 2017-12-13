using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public class NameRequiredException : ApplicationException
    {
        public NameRequiredException(string message) : base(message) {}
    }
}
