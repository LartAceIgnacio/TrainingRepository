using BlastAsia.DigiBook.Domain.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class ContactExtensions
    {

        public static Contact ApplyChanges(this Contact contact,
            Contact from)
        {
            contact.CityAddress = from.CityAddress;
            contact.Country = from.Country;
            contact.StreetAddress = from.StreetAddress;
            contact.EmailAddress = from.EmailAddress;
            contact.MobilePhone = from.MobilePhone;
            contact.ZipCode = from.ZipCode;
            contact.FirstName = from.FirstName;
            contact.LastName = from.LastName;

            return contact;
        }
    }


}
