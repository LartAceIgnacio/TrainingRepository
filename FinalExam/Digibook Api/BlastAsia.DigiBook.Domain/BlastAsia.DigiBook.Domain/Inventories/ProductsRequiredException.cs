using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class ProductsRequiredException: Exception
    {
        public ProductsRequiredException(string Message)
            :base(Message)
        {

        }
    }
}