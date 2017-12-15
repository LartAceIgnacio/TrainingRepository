using System;
using BlastAsia.DigiBook.Domain.Models.Appointments;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
    {
        Appointment Create(Appointment contact);
        Appointment Retrieve(Guid AppointmentId);
        Appointment Update(Guid AppointmentId, Appointment appointment);
    }
}