using System;

namespace BlastAsia.DigiBook.Domain.Test.Contacts.Contacts
{
    public class Contact // Model Properties
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string StreetAddress { get; set; }
        public string CityAddress { get; set; }
        public int ZipCode { get; set; } // Not negative
        public string Country { get; set; }
        public string EmailAddress { get; set; } //Not required
        public bool IsActive { get; set; }
        public DateTime? DateActivated { get; set; } 
        public Guid ContactId { get; set; }
    }
}