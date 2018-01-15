using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        PaginationResult<Appointment> Retrieve(int pageNo, int numRec, string filterValue);
    }
}
