using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class ProductNameTooLongException
        : Exception
    {
        public ProductNameTooLongException(string message)
            : base(message)
        {

        }
    }
}