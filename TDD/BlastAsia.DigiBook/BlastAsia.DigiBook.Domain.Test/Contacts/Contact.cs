using System;

namespace BlastAsia.DigiBook.Domain.Test.Contacts.Contacts
{
    public class Contacts
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string StreetAddress { get; set; }
        public string CityAddress { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateActivated { get; set; }
    }
}