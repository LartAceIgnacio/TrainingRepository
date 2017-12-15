using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments.Services
{
    public interface IAppointmentService
    {
        Appointment Save(Appointment appointment);
    }
}
