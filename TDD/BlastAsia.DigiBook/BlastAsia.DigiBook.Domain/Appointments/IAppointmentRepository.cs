using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;


namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
    {
        Appointement Retrieve(Guid id);
        Appointement Create(Appointement appointment, Guid id, Guid id2);
        Appointement Update(Guid id, Appointement employee);
    }
}