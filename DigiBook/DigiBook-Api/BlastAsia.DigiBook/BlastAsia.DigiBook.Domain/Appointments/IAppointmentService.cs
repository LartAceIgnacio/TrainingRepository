using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentService
    {
        Appointment Save(Guid id,Appointment appointment);
    }
}