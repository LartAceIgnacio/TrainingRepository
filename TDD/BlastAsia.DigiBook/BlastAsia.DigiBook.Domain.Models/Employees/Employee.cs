using System;
using System.IO;

namespace BlastAsia.DigiBook.Domain.Models.Employees
{
    public class Employee
    {
        public Guid EmployeeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string EmailAddress { get; set; }
        public Stream Photo { get; set; }
        public string OfficePhone { get; set; }
        public string Extension { get; set; }
    }
}