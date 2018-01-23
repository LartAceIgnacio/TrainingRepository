using System;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Records;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
        :IRepository<Appointment>
    {
        Record<Appointment> Fetch(int pageNo, int numRec, string filterValue);
    }
}