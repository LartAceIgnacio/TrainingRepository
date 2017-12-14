using System;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Contacts.ContactExceptions;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ContactService : IContactService
    {
        private IContactRepository contactRepository;
        const string rfc2822EmailPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public Contact Save(Contact contact)
        {
            if (string.IsNullOrEmpty(contact.Firstname))
            {
                throw new NameRequiredException("Firstname is required.");
            }

            if (string.IsNullOrEmpty(contact.Lastname))
            {
                throw new NameRequiredException("Lastname is required.");
            }

            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new AddressRequiredException("Address is required.");
            }

            if (string.IsNullOrEmpty(contact.MobilePhone))
            {
                throw new ContactNumberRequiredException("Contact number is required.");
            }

            if (contact.MobilePhone.Length < 11)
            {
                throw new ContactNumberMinimumLength("Contact number must 11 digit length.");
            }

            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new CityAddressRequiredException("City Address is required.");
            }

            if (!contact.ZipCode.HasValue)
            {
                throw new InvalidZipCodeException("Zip code is required.");
            }
            
            if (contact.ZipCode < 0)
            {
                throw new InvalidZipCodeException("Zip code must non-negative value.");
            }

            if (!Regex.IsMatch(contact.EmailAddress, rfc2822EmailPattern))
            {
                throw new InvalidEmailFormatException("Email address is not in correct format.");
            }

            Contact result = null;

            var found = contactRepository.Retrieve(contact.ContactId);

            if (found == null)
            {
                result = contactRepository.Create(contact);
            }
            else
            {
                result = contactRepository.Update(contact.ContactId, contact);
            }

            //result = (found == null) ? contactRepository.Create(contact) : contactRepository.Update(contact.ContactId, contact);

            return result;
        }
        

    }
}