using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Pagination;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
        : IRepository<Appointment>
    {
        Pagination<Appointment> Retrieve(int pageNumber, int recordNumber, DateTime? keyWord);
       // IEnumerable<Appointment> Retrieve(DateTime date);
    }
}