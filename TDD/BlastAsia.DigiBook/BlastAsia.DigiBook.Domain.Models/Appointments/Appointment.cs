using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlastAsia.DigiBook.Domain.Models.Appointments
{
    public class Appointment
    {
        public Guid AppointmentId {get; set;}
        public Guid GuestId { get; set; }
        public Guid HostId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsDone { get; set; }
        public string Notes { get; set; }

        [ForeignKey("GuestId")] // New
        [JsonIgnore] // New
        public Contact Guest { get; set; }

        [ForeignKey("HostId")] // New
        [JsonIgnore] // New
        public Employee Host { get; set; }
    }
}