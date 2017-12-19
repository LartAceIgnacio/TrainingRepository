using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class AppointmentExtension
    {
        public static Appointment ApplyChanges(this Appointment appointment,
            Appointment from)
        {
            appointment.AppointmnetDate = from.AppointmnetDate;
            appointment.GuestID = from.GuestID;
            appointment.HostId = from.HostId;
            appointment.StartTime = from.StartTime;
            appointment.EndTime = from.EndTime;
            appointment.IsCanceleld = from.IsCanceleld;
            appointment.IsDone = from.IsDone;
            appointment.Notes = from.Notes;

            return appointment;
        }
    }
}
