using System;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ContactService
    {
        private readonly int minimumZipcode = 0;

        private IContactRepository contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public Contact Save(Contact contact)
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
                result = contactRepository.Update(contact.ContactId, contact);
            }

            return result;
        }
    }
}