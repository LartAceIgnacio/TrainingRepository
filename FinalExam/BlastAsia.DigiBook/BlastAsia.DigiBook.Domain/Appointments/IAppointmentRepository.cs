using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
        : IRepository<Appointment>
    {
        PaginationResult<Appointment> Retrieve(int page, int record, string filter);
    }
}