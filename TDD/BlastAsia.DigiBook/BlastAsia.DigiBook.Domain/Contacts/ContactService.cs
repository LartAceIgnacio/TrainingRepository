﻿using System;
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
                throw new NameRequiredException("first name is required");
            }
            if (string.IsNullOrEmpty(contact.LastName))
            {
                throw new NameRequiredException("Last name is required");
            }
            if (string.IsNullOrEmpty(contact.MobilePhone))
            {
                throw new MobileNumberRquiredException("Mobile number is required");
            }
            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new StreetAddressRquiredException("Street address is required");
            }
            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new CityAddressRquiredException("City address is required");
            }
            if (contact.ZipCode < 0)
            {
                throw new ZipCodeNegativeException("Zip Code must not have Negative");
            }
            if (string.IsNullOrEmpty(contact.Country))
            {
                throw new CountryRquiredException("Country is required");
            }
            var newContact = contactRepository.Create(contact);

            return newContact;
        }
    }
}