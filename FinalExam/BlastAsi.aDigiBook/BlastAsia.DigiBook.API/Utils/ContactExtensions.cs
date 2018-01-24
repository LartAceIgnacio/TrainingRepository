using BlastAsia.DigiBook.Domain.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class ContactExtensions
    {
        public static Contact ApplyChanges(this Contact contact, Contact from)
        {
            contact.FirstName = from.FirstName;
            contact.LastName = from.LastName;
            contact.MobilePhone = from.MobilePhone;
            contact.StreetAddress = from.StreetAddress;
            contact.CityAddress = from.CityAddress;
            contact.ZipCode = from.ZipCode;
            contact.EmailAddress = from.EmailAddress;
            contact.IsActive = from.IsActive;
            contact.DateActivated = from.DateActivated;

            return contact;
        }
    }
}
