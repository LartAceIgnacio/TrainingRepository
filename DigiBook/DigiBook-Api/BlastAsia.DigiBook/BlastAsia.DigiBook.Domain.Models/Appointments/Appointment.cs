using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System;

namespace BlastAsia.DigiBook.Domain.Models.Appointments
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public DateTime? DateActivated { get; set; }
        public Guid GuestId { get; set; }
        public Guid HostId { get; set; }
        public Contact Guest { get; set; }
        public Employee Host { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsDone { get; set; }
        public string Notes { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}