using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace BlastAsia.Digibook.Domain.Models.Employees
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string MobilePhone { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        //[Required]
        public Stream Photo { get; set; }

        [Required]
        public string OfficePhone { get; set; }

        [Required]
        public string Extension { get; set; }

        public Byte[] PhotoByte { get; set; }
    }
}
