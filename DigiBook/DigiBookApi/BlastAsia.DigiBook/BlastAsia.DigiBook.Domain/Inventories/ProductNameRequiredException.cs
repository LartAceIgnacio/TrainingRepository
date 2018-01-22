using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class ProductNameRequiredException
        : Exception
    {
        public ProductNameRequiredException(string message)
            : base(message)
        {

        }
    }
}