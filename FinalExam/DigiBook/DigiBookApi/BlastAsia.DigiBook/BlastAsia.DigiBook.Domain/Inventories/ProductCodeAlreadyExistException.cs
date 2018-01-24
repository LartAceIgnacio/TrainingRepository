using System;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class ProductCodeAlreadyExistException
        : Exception
    {
        public ProductCodeAlreadyExistException(string message)
            : base(message)
        {

        }
    }
}