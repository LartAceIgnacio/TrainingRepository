using BlastAsia.DigiBook.Domain.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class ContactExtension
    {
        public static Contact ApplyNewChanges(this Contact oldContact, Contact newContact)
        {
            oldContact.FirstName = newContact.FirstName;
            oldContact.LastName = newContact.LastName;
            oldContact.MobilePhone = newContact.MobilePhone;
            oldContact.StreetAddress = newContact.StreetAddress;
            oldContact.CityAddress = newContact.CityAddress;
            oldContact.Country = newContact.Country;
            oldContact.EmailAddress = newContact.EmailAddress;
            oldContact.IsActive = newContact.IsActive;
            oldContact.DateActivated = newContact.DateActivated;

            return oldContact;
        }
    }
}
