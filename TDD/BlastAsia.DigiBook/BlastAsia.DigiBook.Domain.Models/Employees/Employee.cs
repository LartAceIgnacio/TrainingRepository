using System;
using System.IO;

namespace BlastAsia.DigiBook.Domain.Models.Employees
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public Stream Photo { get; set; }
        public string OfficePhone { get; set; }
        public string Extension { get; set; }
        public Guid EmployeeId { get; set; }
    }
}