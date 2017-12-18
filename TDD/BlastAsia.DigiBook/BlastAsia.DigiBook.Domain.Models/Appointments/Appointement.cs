using System;

namespace BlastAsia.DigiBook.Domain.Models.Appointments
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public DateTime? AppointmnetDate { get; set; }
        public Guid GuestID { get; set; }
        public Guid HostId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool IsCanceleld { get; set; }
        public bool IsDone { get; set; }
        public string Notes { get; set; }
    }
}