using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
        : IRepository<Appointment>
    {
        Pagination<Appointment> Retrieve(int pageNo, int numRec, string filterValue);
    }
}