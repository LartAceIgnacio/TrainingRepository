using System;
using BlastAsia.Digibook.Domain.Models.Contacts;

namespace BlastAsia.Digibook.Domain.Contacts
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
                throw new MobilePhoneRequiredException();
            }
            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new AddressRequiredException("Street address is required.");
            }
            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new AddressRequiredException("City address is required.");
            }
            if (contact.ZipCode < 0)
            {
                throw new ZipNegativeNumberException();
            }
            if (string.IsNullOrEmpty(contact.Country))
            {
                throw new AddressRequiredException("Country is required.");
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
                result = contactRepository.Update(contact.ContactId, contact);
            }

            return result;
        }
    }
}