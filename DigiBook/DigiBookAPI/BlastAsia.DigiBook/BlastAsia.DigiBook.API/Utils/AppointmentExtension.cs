using BlastAsia.DigiBook.Domain.Models.Appointments;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class AppointmentExtension
    {
        public static Appointment ApplyChanges(
            this Appointment appointment
            , Appointment form)
        {
            appointment.AppointmentDate = form.AppointmentDate;
            appointment.EndTime = form.EndTime;
            appointment.GuestId = form.GuestId;
            appointment.HostId = form.HostId;
            appointment.IsCancelled = form.IsCancelled;
            appointment.IsDone = form.IsDone;
            appointment.Notes = form.Notes;
            appointment.StartTime = form.StartTime;

            return appointment;
        }
    }
}