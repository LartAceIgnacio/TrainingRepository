using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFTraining.Data.Models
{
    public class Contact
    {
        public Guid ContactId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public List<Appointment> Appointments { get; set; }

        public string MiddleName { get; set; }
    }
}