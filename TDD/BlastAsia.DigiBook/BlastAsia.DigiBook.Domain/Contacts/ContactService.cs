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

        public Contact Create(Contact contact)
        {
            if (string.IsNullOrEmpty(contact.FirstName))
            {
                throw new NameRequiredException("Firstname is required");
            }
            if (string.IsNullOrEmpty(contact.LastName))
            {
                throw new NameRequiredException("Lastname is required");
            }
            if (string.IsNullOrEmpty(contact.MobilePhone))
            {
                throw new MobilePhoneRequiredException("Mobilephone is required");
            }
            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new AddressRequiredException("Street address is required");
            }
            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new AddressRequiredException("City address is required");
            }
            if (contact.ZipCode < 0)
            {
                throw new AddressRequiredException("Zipcode is invalid");
            }
            if (string.IsNullOrEmpty(contact.Country))
            {
                throw new AddressRequiredException("Country is required");
            }
            var newContact = contactRepository.Create(contact);

            return newContact;
        }
    }
}