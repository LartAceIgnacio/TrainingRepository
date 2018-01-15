using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlastAsia.DigiBook.Domain.Models.Appointments
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public Guid GuestId { get; set; }
        [ForeignKey("HostId")]
        [JsonIgnore]
        public Employee Host { get; set; }
        [ForeignKey("GuestId")]
        [JsonIgnore]
        public Contact Guest { get; set; }
        public Guid HostId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsDone { get; set; }
        public string Notes { get; set; }

    }
}