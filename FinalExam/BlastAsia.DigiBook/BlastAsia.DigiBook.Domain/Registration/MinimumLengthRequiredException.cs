using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public class MinimumLengthRequiredException
        : Exception
    {
        public MinimumLengthRequiredException(string message) : base(message)
        {
        }
    }
}
