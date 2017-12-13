using System;
using System.Collections.Generic;
using System.Text;


namespace BlastAsia.DigiBook.Domain.Models.Contacts
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string StreetAddress { get; set; }
        public string CityAddress { get; set; }
        public int ZipCode { get; set; }
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateActivated { get; set; }
        public Guid ContactId { get; set; }
    }
}