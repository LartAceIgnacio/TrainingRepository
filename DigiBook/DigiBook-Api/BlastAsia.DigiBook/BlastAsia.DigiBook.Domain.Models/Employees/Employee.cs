using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Employees
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string EmailAddress { get; set; }
        public Stream Photo { get; set; }
        public string OfficePhone { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateActivated { get; set; }
        public Guid EmployeeId { get; set; }
        public string Extension { get; set; }
        public bool IsCancel { get; set; }
        public bool IsDone { get; set; }
        public string Notes { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
