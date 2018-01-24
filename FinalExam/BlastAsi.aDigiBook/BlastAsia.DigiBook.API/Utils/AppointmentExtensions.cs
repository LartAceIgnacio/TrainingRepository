using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class AppointmentExtensions
    {
        public static Appointment ApplyChanges(this Appointment appointment, Appointment from)
        {
            appointment.AppointmentDate = from.AppointmentDate;
            appointment.GuestId = from.GuestId;
            appointment.HostId = from.HostId;
            appointment.StartTime = from.StartTime;
            appointment.EndTime = from.EndTime;
            appointment.IsCancelled = from.IsCancelled;
            appointment.IsDone = from.IsDone;
            appointment.Notes = from.Notes;

            return appointment;
        }
    }
}
