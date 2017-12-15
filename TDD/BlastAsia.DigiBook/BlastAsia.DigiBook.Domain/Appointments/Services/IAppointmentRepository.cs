using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments.Services
{
    public interface IAppointmentRepository
    {
        Appointment Retrieve(Guid appointmentId);

        Appointment Create(Appointment appointment);
    }
}