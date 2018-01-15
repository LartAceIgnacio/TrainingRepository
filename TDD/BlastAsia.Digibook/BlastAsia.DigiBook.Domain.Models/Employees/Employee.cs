using BlastAsia.DigiBook.Domain.Models.Appointments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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
        public string PhotoUrl { get; set; }
        public string OfficePhone { get; set; }
        public string Extension { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
    }
}