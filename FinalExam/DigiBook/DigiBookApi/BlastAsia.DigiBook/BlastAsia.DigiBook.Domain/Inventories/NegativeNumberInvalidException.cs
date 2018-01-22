using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class NegativeNumberInvalidException
        : Exception
    {
        public NegativeNumberInvalidException(string message)
            : base(message)
        {

        }
    }
}