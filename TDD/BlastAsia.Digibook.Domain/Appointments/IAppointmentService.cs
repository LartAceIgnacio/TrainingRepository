using BlastAsia.Digibook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Domain.Appointments
{
    public interface IAppointmentService
    {
        Appointment Set(Appointment appointment);
    }
}
