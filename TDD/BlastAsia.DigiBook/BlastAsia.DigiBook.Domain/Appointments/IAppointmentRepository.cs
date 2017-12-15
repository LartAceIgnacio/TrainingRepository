using System;
using BlastAsia.DigiBook.Domain.Models.Appointments;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
    {
        Appointment Create(Appointment appointment);
        Appointment Retrieve(Guid existingGuid);
        Appointment Update(Guid appointmentId, Appointment appointment);
    }
}