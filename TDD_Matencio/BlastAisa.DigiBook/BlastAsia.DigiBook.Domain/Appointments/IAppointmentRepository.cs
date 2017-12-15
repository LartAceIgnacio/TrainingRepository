using System;
using BlastAsia.DigiBook.Domain.Models.Appointments;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
    {
        Appointment Create(Appointment appointment);
        Appointment Retrieve(Guid appointmentId);
        Appointment Update(Guid id, Appointment appointment);
    }
}