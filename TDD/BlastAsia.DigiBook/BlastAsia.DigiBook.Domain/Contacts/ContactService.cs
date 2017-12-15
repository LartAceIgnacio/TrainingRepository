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
                throw new NameRequiredException("Firstname is required!");
            }
            if (string.IsNullOrEmpty(contact.LastName))
            {
                throw new NameRequiredException("LastName is required!");
            }
            if (string.IsNullOrEmpty(contact.MobilePhone))
            {
                throw new MobilePhoneRequiredException("MobilePhone is required");
            }
            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new StrongAddressRequiredException("StreetAddress is required!");
            }
            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new StrongAddressRequiredException("CityAddress is required!");
            }
            if (contact.ZipCode < 0)
            {
                throw new ZipCodeRequiredException("Positive number in ZipCode is required!");
            }
            if (string.IsNullOrEmpty(contact.Country))
            {
                throw new StrongAddressRequiredException("Country is required!");
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
                    .Update(contact.ContactId,contact);
            }

            return result;
        }
    }
}