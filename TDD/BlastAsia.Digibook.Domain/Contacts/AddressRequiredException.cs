using System;

namespace BlastAsia.Digibook.Domain.Contacts
{
    public class AddressRequiredException:Exception
    {
        public AddressRequiredException(string message) : base(message)
        {

        }
    }
}