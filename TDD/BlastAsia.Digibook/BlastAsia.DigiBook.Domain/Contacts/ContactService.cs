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

        public Contact Save(Contact contact)
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
                throw new InvalidEmailException("Invalid email format");
            }

            Contact result = null;

            var found = _contactRepository.Retrieve(contact.ContactId);

            #region Long If else
            //if(found == null)
            //{
            //    result = _contactRepository.Create(contact);
            //} else
            //{
            //    result = _contactRepository.Update(contact.ContactId, contact);
            //}
            #endregion

            result = found == null ? _contactRepository.Create(contact) : _contactRepository.Update(contact.ContactId, contact);

            return result;
        }
    }
}