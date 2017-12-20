using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Api.Utils
{
    public static class ContactExtensions
    {
        public static Contact ApplyChanges(
            this Contact contact,
            Contact form)
        {
            contact.CityAddress = form.CityAddress;
            contact.Country = form.Country;
            contact.StreetAddress = form.StreetAddress;
            contact.EmailAddress = form.EmailAddress;
            contact.MobilePhone = form.MobilePhone;
            contact.ZipCode = form.ZipCode;
            contact.FirstName = form.FirstName;
            contact.LastName = form.LastName;

            return contact;
        }
    }
}