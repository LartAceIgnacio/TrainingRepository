using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class ProductDescriptionMaxLengthException: Exception
    {
        public ProductDescriptionMaxLengthException(string message)
            :base(message)
        {

        }
    }
}