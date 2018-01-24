using BlastAsia.DigiBook.Domain.Models.Appointments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Employees
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobilePhone { get; set; }

        public string EmailAddress { get; set; }

        public Stream Photo { get; set; }

        public string OfficePhone { get; set; }

        public string Extension { get; set; }

        [MaxLength(16)]
        //public byte[] PhotoByte { get; set; }

        [JsonIgnore]
        public List<Appointment> Appointments { get; set; }
    }
}