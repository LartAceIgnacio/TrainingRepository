using System;

namespace BlastAsia.DigiBook.Domain.Models.Appointments
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid GuestId { get; set; }
        public Guid HostId { get; set; }
        public Func<DateTime> StartTime { get; set; }
        public Func<DateTime> EndTime { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsDone { get; set; }
        public string Notes { get; set; }
    }
}