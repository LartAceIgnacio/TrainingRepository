using System;
using System.ComponentModel.DataAnnotations;

namespace BlastAsia.Digibook.Domain.Models.Appointments
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public Guid GuestId { get; set; }

        [Required]
        public Guid HostId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public bool IsCanceled { get; set; }

        [Required]
        public bool IsDone { get; set; }

        public string Notes { get; set; }
    }
}