using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Paginations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Pagination<Appointment> Retrieve(int pageNumber, int recorNumber, DateTime? search);
    }
}
