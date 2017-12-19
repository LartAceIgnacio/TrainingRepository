using System;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ContactService : IContactService
    {
        private readonly int minimumZipcode = 0;

        private IContactRepository contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public Contact Save(Guid id, Contact contact)
        {
            if(string.IsNullOrEmpty(contact.FirstName))
            {
                throw new NameRequiredException("First Name is required.");
            }
            if(string.IsNullOrEmpty(contact.LastName))
            {
                throw new NameRequiredException("Last Name is required.");
            }
            if(string.IsNullOrEmpty(contact.MobilePhone))
            {
                throw new MobilePhoneRequiredException("Mobile phone is required.");
            }
            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new AddressRequiredException("Street Address is required.");
            }
            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new AddressRequiredException("City Address is required.");
            }
            if(string.IsNullOrEmpty(contact.Country))
            {
                throw new CountryRequiredException("Country is required");
            }
            if(contact.ZipCode < minimumZipcode)
            {
                throw new ZipcodeShouldBePositiveException("Zipcode should be positive.");
            }

            Contact result = null;

            var found = contactRepository.Retrieve(contact.ContactId);
            if (found == null)
            {
                result = contactRepository.Create(contact);
            }
            else
            {
                //result = contactRepository.Update(contact.ContactId, contact);
                found.FirstName = contact.FirstName;
                found.LastName = contact.LastName;
                found.MobilePhone = contact.MobilePhone;
                found.StreetAddress = contact.StreetAddress;
                found.CityAddress = contact.CityAddress;
                found.Country = contact.Country;
                found.EmailAddress = contact.EmailAddress;
                found.isActive = contact.isActive;
                found.DateActivated = contact.DateActivated;
                result = contactRepository.Update(found.ContactId, found);
            }

            return result;
        }
    }
}