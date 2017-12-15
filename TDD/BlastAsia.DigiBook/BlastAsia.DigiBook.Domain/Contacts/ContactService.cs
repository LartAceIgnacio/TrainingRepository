using System;
using BlastAsia.DigiBook.Domain.Test.Contacts.Contacts;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public class ContactService
    {
        private IContactRepository contactRepository;

        public ContactService(IContactRepository contactRepository) // Inject
        {
            this.contactRepository = contactRepository;
        }

        //private readonly string emailregex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
        //@"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
        //@".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public Contact Save(Contact contact)
        {
            //throw new NotImplementedException();
            
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
                throw new MobilePhoneRequiredException("Mobile number is required");
            }
            if (string.IsNullOrEmpty(contact.StreetAddress))
            {
                throw new AddressRequiredException("Street Address is required");
            }
            if (string.IsNullOrEmpty(contact.CityAddress))
            {
                throw new AddressRequiredException("Street Address is required");
            }
            if (contact.ZipCode <= 0)
            {
                throw new ZipCodeException("Non Negative Zip Code is required");
            }
            if (string.IsNullOrEmpty(contact.Country))
            {
                throw new CountryRequiredException("Country is required");
            }
            //if (!string.IsNullOrEmpty(contact.EmailAddress))
            //{
            //    if (!Regex.IsMatch(contact.EmailAddress, emailregex, RegexOptions.IgnoreCase))
            //    {
            //        throw new ValidEmailRequiredException("Correct Email Format Required");
            //    }
            //}
            Contact result = null;
            var found = contactRepository
                .Retrieve(contact.ContactId);

            if (found == null)
            {
                result = contactRepository
                    .Create(contact);
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