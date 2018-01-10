using System;
using System.Collections.Generic;

namespace EFTraining.Data.Models
{
    public class Contact
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; }
       // public string MiddleName { get; set; }         
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
