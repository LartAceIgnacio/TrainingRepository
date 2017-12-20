using BlastAsia.Digibook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.Digibook.API.Utils
{
    public static class AppointementExtensions
    {
        public static Appointment ApplyAppointmentChanges(this Appointment sourceAppointment, Appointment destinationAppointment)
        {
            destinationAppointment.AppointmentDate = sourceAppointment.AppointmentDate;
            destinationAppointment.GuestId = sourceAppointment.GuestId;
            destinationAppointment.HostId = sourceAppointment.HostId;
            destinationAppointment.StartTime = sourceAppointment.StartTime;
            destinationAppointment.EndTime = sourceAppointment.EndTime;
            destinationAppointment.IsCanceled = sourceAppointment.IsCanceled;
            destinationAppointment.IsDone = sourceAppointment.IsDone;
            destinationAppointment.Notes = sourceAppointment.Notes;

            return destinationAppointment;
        }
    }
}
