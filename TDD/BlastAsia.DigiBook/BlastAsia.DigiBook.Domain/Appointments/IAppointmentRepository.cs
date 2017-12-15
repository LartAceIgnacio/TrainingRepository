using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
        : IRepository<Appointment>
    {
        Appointment Create(Guid EmployeeId, Guid ContactId, Appointment appointment);
    }
}