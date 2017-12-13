using System;
using BlastAsia.DigiBook.Domain.Contacts.Interfaces;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Contacts.Exceptions;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ContactService
    {
        private IContactRepository contactRepository;
        private readonly string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public Contact Create(Contact contact)
        {
            if (string.IsNullOrEmpty(contact.FirstName))
            {
                throw new NameRequiredException("Firstname is required!");
            }
            if(string.IsNullOrEmpty(contact.LastName))
            {
                throw new NameRequiredException("Lastname is required!");
            }
            if (string.IsNullOrEmpty(contact.MobilePhone))
            {
                throw new MobilePhoneRequiredException("Mobilephone is required!");
            }
            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new AddressRequiredException("Street Address is required!");
            }
            if(contact.ZipCode < 0)
            {
                throw new AddressRequiredException("Zipcode is required!");
            }
            if (string.IsNullOrEmpty(contact.Country))
            {
                throw new AddressRequiredException("Country is required!");
            }
            if (!(string.IsNullOrWhiteSpace(contact.EmailAddress)))
            {
                if (!Regex.IsMatch(contact.EmailAddress, strRegex, RegexOptions.IgnoreCase))
                throw new InvalidEmailAddressException("Valid Email address is required!");
            }

            var newContact = contactRepository.Create(contact);
            return newContact;
        }
        
    }
}