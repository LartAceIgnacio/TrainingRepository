using System;
using BlastAsia.DigiBook.Domain.Models.Appointments;

namespace BlastAsia.DigiBook.Domain
{
    public interface IAppointmentService
    {
        Appointment Save(Guid id, Appointment appointment);
    }
}