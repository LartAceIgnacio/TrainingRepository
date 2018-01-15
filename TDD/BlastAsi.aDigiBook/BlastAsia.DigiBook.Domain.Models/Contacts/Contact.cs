using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Domain.Models.Contacts
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string StreetAddress { get; set; }
        public string CityAddress { get; set; }
        public int ZipCode { get; set; } //not negative
        public string Country { get; set; }
        public string EmailAddress { get; set; } //not required
        public bool IsActive { get; set; }
        public DateTime? DateActivated { get; set; } //nullable , pwedeng walang value
        public Guid ContactId { get; set; } //cannot be set
        public List<Appointment> Appointments { get; set; }
    }
}