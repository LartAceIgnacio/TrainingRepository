using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Appointments
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        //[ForeignKey("Contact")]
        public Guid GuestId { get; set; }
        //public Contact Contact { get; set; }

        //[ForeignKey("Employee")]
        public Guid HostId { get; set; }
        //public Employee Employee { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsDone { get; set; }

        public string Notes { get; set; }
    }
}
