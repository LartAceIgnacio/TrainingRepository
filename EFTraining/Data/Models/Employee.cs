using System;
using System.Collections.Generic;

namespace EFTraining.Data.Models
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string OfficePhone { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}