using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ZipCodeNegativeException
        :Exception
    {
        public ZipCodeNegativeException(string message)
            :base(message)
        {

        }
    }
}