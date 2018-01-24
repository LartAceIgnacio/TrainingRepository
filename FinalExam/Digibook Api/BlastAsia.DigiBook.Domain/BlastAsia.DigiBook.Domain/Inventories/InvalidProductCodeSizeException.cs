using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class InvalidProductCodeSizeException: Exception
    {
        public InvalidProductCodeSizeException(string Message)
            :base(Message)
        {

        }
    }
}