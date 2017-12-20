using System;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ContactService: IContactService
    {
        private IContactRepository contactRepository;
        private readonly string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public Contact Save(Guid id, Contact contact)
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
                throw new MobilePhoneRequiredException("MobilePhone is required");
            }
            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new AddressRequiredException("Street Addreess is required");
            }
            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new AddressRequiredException("City Address is required");
            }
            if (contact.ZipCode < 0)
            {
                throw new NonNegativeZipCodeException("ZipCode should not be negative");
            }
            if (string.IsNullOrEmpty(contact.Country))
            {
                throw new AddressRequiredException("Country is required");
            }
            if (!Regex.IsMatch(contact.EmailAddress, pattern, RegexOptions.IgnoreCase) && !string.IsNullOrWhiteSpace(contact.EmailAddress))
            {
                throw new EmailInvalidFormatException();
            }

            Contact result = null;
            var found = contactRepository
                .Retrieve(contact.ContactId);
            

            if(found == null)
            {
                result = contactRepository.Create(contact);
            }
            else
            {
                result = contactRepository
                    .Update(contact.ContactId, contact);
            }

            
            return result;
        }
    }
}