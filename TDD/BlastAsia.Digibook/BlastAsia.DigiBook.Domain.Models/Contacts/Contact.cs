using System;

namespace BlastAsia.DigiBook.Domain.Models.Contacts
{
    public class Contact
    {
        public string FirstName { get; set; } // NameRequired Exception
        public string LastName { get; set; } // NameRequired Exception
        public string MobilePhone { get; set; }
        public string StreetAddress { get; set; }
        public string CityAddress { get; set; }
        public int ZipCode { get; set; } // not negative required 
        public string Country { get; set; }
        public string EmailAddress { get; set; } // not
        public bool IsActive { get; set; }
        public DateTime? DateActivated { get; set; } // not
        public Guid ContactId { get; set; }
    }
}