using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class ProductCodeInvalidException
        : Exception
    {
        public ProductCodeInvalidException(string message)
            : base(message)
        {

        }
    }
}