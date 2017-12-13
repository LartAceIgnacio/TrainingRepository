using System;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Contacts.Exception;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ContactService
    {
        private IContactRepository _contactRepository;
        private readonly string regex = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        public ContactService (IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public Contact Create(Contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.FirstName))
            {
                throw new NameRequiredException("First Name is required");
            }

            if (string.IsNullOrWhiteSpace(contact.LastName))
            {
                throw new NameRequiredException("Last Name is required");
            }

            if (string.IsNullOrWhiteSpace(contact.MobilePhone))
            {
                throw new MobilePhoneRequiredException("Mobile Phone is required");
            }

            if (string.IsNullOrWhiteSpace(contact.StreetAddress))
            {
                throw new StreetAddressRequiredException("Street Address is required");
            }


            if (string.IsNullOrWhiteSpace(contact.CityAddress))
            {
                throw new CityAddressRequiredException("City Address is required");
            }

            if (contact.ZipCode == 0)
            {
                throw new ZipCodeRequiredException("Zip Code is required");
            }

            if (contact.ZipCode < 0)
            {
                throw new NegativeZipCodeException("Zip Code should not be negative");
            }

            if (string.IsNullOrWhiteSpace(contact.Country))
            {
                throw new CountryRequiredException("Country is required");
            }

            // Check email format if correct
            if ((!string.IsNullOrWhiteSpace(contact.EmailAddress)) && (!Regex.IsMatch(contact.EmailAddress, regex)))
            {
                throw new InvalidEmailException("Invali email format");
            }

            var newContact = _contactRepository.Create(contact);
            return newContact;
        }
    }
}