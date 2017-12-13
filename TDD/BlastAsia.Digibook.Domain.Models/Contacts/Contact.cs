using System;
using System.ComponentModel.DataAnnotations;

namespace BlastAsia.Digibook.Domain.Models.Contacts
{
    public class Contact
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string MobilePhone { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string CityAddress { get; set; }

        [Range(0,int.MaxValue)]
        public int ZipCode { get; set; }

        [Required]
        public string Country { get; set; }

        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public DateTime? DateActive { get; set; }

        public Guid ContactId { get; set; }
    }
}