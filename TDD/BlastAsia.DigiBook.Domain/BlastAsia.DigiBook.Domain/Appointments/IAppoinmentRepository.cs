using BlastAsia.DigiBook.Domain.Models.Appoinments;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppoinmentRepository
    {
        Appoinment Retrieve(Guid id);

        Appoinment Create(Appoinment appoinment);

        Appoinment Update(Guid appointmentId, Appoinment appoinment);
    }
}