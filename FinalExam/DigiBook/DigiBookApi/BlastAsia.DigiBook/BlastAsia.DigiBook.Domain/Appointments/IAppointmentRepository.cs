using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
        : IRepository<Appointment>
    {
        PaginationClass<Appointment> Retrieve(int pageNo, int numRec, string filterValue);
    }
}