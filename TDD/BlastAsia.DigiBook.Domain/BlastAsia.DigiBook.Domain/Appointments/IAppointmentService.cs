using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentService
    {
        Appointment Save(Guid id, Appointment appointment);
    }
}