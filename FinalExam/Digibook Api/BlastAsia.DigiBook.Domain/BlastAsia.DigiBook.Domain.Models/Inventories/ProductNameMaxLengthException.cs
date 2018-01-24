using System;

namespace BlastAsia.DigiBook.Domain.Models.Inventories
{
    public class ProductNameMaxLengthException: Exception
    {
        public ProductNameMaxLengthException(string Message)
            :base(Message)
        {

        }
    }
}