using System;
using BlastAsia.DigiBook.Domain.Models.Appointments;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentService
    {
        Appointment Save(Guid id, Appointment appointment);
    }
}