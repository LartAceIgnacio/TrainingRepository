using System;
using BlastsAsia.DigiBook.Domain.Models.Contacts;

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

            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new CityAddressRequiredException("City Address is required.");
            }

            if ( !contact.ZipCode.HasValue)
            {
                throw new InvalidZipCodeException("Zip code is required.");
            }
            
            if (contact.ZipCode < 0)
            {
                throw new InvalidZipCodeException("Zip code must non-negative value.");
            }

            var newcontact = contactRepository.Create(contact);

            return newcontact;
        }
        

    }
}