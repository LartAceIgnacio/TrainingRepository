using System;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ContactService
    {
        private IContactRepository contactRepository;
        private readonly string regex = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public Contact Create(Contact contact)
        {
            if(string.IsNullOrEmpty(contact.FirstName) || string.IsNullOrEmpty(contact.LastName)) {
                throw new NameRequiredException(string.IsNullOrEmpty(contact.LastName) ? "Last Name is Required" : "First Name is Required");
            }

            if (string.IsNullOrEmpty(contact.MobilePhone)) {
                throw new MobilePhoneRequiredException("Mobile Phone is Required");
            }

            if (string.IsNullOrEmpty(contact.StreetAddress)) {
                throw new AddressRequiredException("Street Address is Required");
            }

            if (string.IsNullOrEmpty(contact.CityAddress)) {
                throw new AddressRequiredException("City Address is Required");
            }

            if (contact.ZipCode == 0) {
                throw new AddressRequiredException("Zip Code is Required");
            }

            if (contact.ZipCode < 0) {
                throw new NagativeZipCodeException("Zip Code can't be ngative");
            }

            if (string.IsNullOrEmpty(contact.Country)) {
                throw new AddressRequiredException("Country is Required");
            }

            if (!Regex.IsMatch(contact.EmailAddress, regex)) {
                throw new InvalidEmailAddressException("Invalid Email Address");
            }

            var newContact = contactRepository.Create(contact);
            return newContact;

        }
    }
}