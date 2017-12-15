using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public interface IAppointmentRepository
    {
        Appointment Create(Appointment appointment);
        Appointment Retrieve(Guid guestId);
        Appointment Retrieve(Appointment appointmentId);
        Appointment Update(Guid appointmentId);
    }
}
