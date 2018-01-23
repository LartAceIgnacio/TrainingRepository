﻿using BlastAsia.DigiBook.Domain.Models.Appointments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlastAsia.DigiBook.Domain.Models.Employees
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string EmailAddress { get; set; }
        public string Photo { get; set; }
        public string OfficePhone { get; set; }
        public string Extension { get; set; }
        public Guid EmployeeId { get; set; }
        [JsonIgnore]
        public List<Appointment> Appointments { get; set; }
    }
}