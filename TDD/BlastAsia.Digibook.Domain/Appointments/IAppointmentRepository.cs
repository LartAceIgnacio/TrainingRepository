using BlastAsia.Digibook.Domain.Models.Appointments;
using System;

namespace BlastAsia.Digibook.Domain.Appointments
{
    public interface IAppointmentRepository
    {
        Appointment Retrieve(Guid appointmentId);

        Appointment Create(Appointment appointment);

        Appointment Update(Appointment appointment, Guid appointmentId);
    }
}