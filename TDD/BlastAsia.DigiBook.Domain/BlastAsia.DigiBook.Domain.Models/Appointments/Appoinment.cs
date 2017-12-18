using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Appoinments
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid GuestId { get; set; }
        public Guid HostId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsDone { get; set; }
        public string Notes { get; set; }
    }
}
