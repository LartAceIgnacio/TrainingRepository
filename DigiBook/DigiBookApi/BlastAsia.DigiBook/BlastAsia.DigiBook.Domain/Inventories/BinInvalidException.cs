using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class BinInvalidException
        : Exception
    {
        public BinInvalidException(string message)
            : base(message)
        {

        }
    }
}