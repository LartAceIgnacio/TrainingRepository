using System;
using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ContactService
    {
        private IContactRepository contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public Contact Save(Contact contact)
        {

            if (string.IsNullOrEmpty(contact.FirstName))
            {
                throw new NameRequiredException("Firstname is required.");
            }
            if (string.IsNullOrEmpty(contact.LastName))
            {
                throw new NameRequiredException("Lastname is required.");
            }
            if (string.IsNullOrEmpty(contact.MobilePhone))
            {
                throw new MobileNumberRequiredException("Phone number is required.");
            }
            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new AddressRequiredException("Street Address is required.");
            }
            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new AddressRequiredException("City Address is required.");
            }
            if (contact.ZipCode < 0)
            {
                throw new PositiveZipCodeRequiredException("Zip Code must be a positive number.");
            }
            if (string.IsNullOrEmpty(contact.Country))
            {
                throw new CountryRequiredException("Country is required.");
            }

            Contact result = null;
            var found = contactRepository
                .Retrieve(contact.ContactId);

            if (found == null)
            {
                result = contactRepository.Create(contact);
            }
            else
            {
                result = contactRepository
                    .Update(contact.ContactId, contact);
            }
            return result;

            //var existingContact = contactRepository
                //.Retrieve(contact.ContactId);

            //var newContact = contactRepository.Create(contact);

           //return newContact;
        }
    }
}