using System;
using System.Runtime.Serialization;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    [Serializable]
    public class ProductCodeShouldUniqueException : Exception
    {
        public ProductCodeShouldUniqueException(string message) 
            : base(message)
        {
        }
    }
}