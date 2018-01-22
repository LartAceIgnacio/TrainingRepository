using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class ProductDescriptionTooLongException
        : Exception
    {
        public ProductDescriptionTooLongException(string message)
            : base(message)
        {

        }
    }
}