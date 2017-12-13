using System;

namespace BlastsAsia.DigiBook.Domain.Models.Contacts
{
    public class Contact
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string MobilePhone { get; set; }
        public string StreetAddress { get; set; }
        public string CityAddress { get; set; }
        public int? ZipCode { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateActivated { get; set; }
        public Guid ContactId { get; set; }
    }
}