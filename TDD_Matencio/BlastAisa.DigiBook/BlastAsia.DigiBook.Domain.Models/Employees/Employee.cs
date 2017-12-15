using System;
using System.IO;

namespace BlastAsia.DigiBook.Domain.Models.Employees
{
    public class Employee
    {
        public Guid employeeId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobilePhone { get; set; }
        public string emailAddress { get; set; }
        
        public string officePhone { get; set; }
        public string extension { get; set; }
        public MemoryStream photo { get; set; }
    }
}