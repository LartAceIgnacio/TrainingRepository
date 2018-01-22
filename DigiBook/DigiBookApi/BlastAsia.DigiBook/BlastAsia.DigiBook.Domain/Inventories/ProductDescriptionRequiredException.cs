using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class ProductDescriptionRequiredException
        : Exception
    {
        public ProductDescriptionRequiredException(string message)
            : base(message)
        {

        }
    }
}