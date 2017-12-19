using System;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ContactService : IContactService
    {
        private IContactRepository contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        private readonly string validEmail = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public Contact Save(Guid id, Contact contact)
        {
            //business rules
            if (string.IsNullOrEmpty(contact.FirstName))
            {
                throw new NameRequiredException("First name is required");
            }
            if (string.IsNullOrEmpty(contact.LastName))
            {
                throw new NameRequiredException("Last name is required");
            }
            if (string.IsNullOrEmpty(contact.MobilePhone))
            {
                throw new MobileNumberRequiredException("Mobile phone is required");
            }
            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new AddressRequiredException("Street address is required");
            }
            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new AddressRequiredException("City address is required");
            }
            if (contact.ZipCode == 0)
            {
                throw new AddressRequiredException("Zip code is required");
            }
            if (string.IsNullOrEmpty(contact.Country))
            {
                throw new AddressRequiredException("Country is required");
            }
            if (contact.ZipCode < 0)
            {
                throw new ValidZipCodeRequiredException("Valid zip code is required");
            }
            if (!Regex.IsMatch(contact.EmailAddress, validEmail, RegexOptions.IgnoreCase))
            {
                throw new EmailAddressRequiredException("Valid email address required");
            }

            Contact result = null;
            var found = contactRepository
                .Retrieve(contact.ContactId);

            if(found == null)
            {
                result = contactRepository.Create(contact);
            }
            else
            {
                found.FirstName = contact.FirstName;
                found.LastName = contact.LastName;
                found.MobilePhone = contact.MobilePhone;
                found.StreetAddress = contact.StreetAddress;
                found.CityAddress = contact.CityAddress;
                found.Country = contact.Country;
                found.EmailAddress = contact.EmailAddress;
                found.IsActive = contact.IsActive;
                found.DateActivated = contact.DateActivated;
                result = contactRepository.Update(found.ContactId, found);
            }
            return result;
        }
    }
}