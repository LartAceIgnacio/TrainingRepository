using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
    {
        Appointment Create(Appointment appointment);
        Appointment Retrieve(Guid  id);
        Appointment Update(Guid appointmentId, Appointment appointment);
    }
}