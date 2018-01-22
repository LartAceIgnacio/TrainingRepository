using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class AppointmentExtension
    {
        public static Appointment ApplyNewChanges(this Appointment oldAppointment, Appointment newAppointment)
        {
            oldAppointment.AppointmentDate = newAppointment.AppointmentDate;
            oldAppointment.EndTime = newAppointment.EndTime;
            oldAppointment.GuestId = newAppointment.GuestId;
            oldAppointment.HostId = newAppointment.HostId;
            oldAppointment.IsCancelled = newAppointment.IsCancelled;
            oldAppointment.IsDone = newAppointment.IsDone;
            oldAppointment.Notes = newAppointment.Notes;
            oldAppointment.StartTime = newAppointment.StartTime;

            return oldAppointment;
        }
    }
}
