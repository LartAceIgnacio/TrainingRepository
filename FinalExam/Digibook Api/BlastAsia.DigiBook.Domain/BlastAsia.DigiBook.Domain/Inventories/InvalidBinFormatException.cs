using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class InvalidBinFormatException: Exception
    {
        public InvalidBinFormatException(string message)
            :base(message)
        {

        }
    }
}