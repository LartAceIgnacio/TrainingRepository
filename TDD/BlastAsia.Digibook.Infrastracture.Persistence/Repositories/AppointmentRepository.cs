using BlastAsia.Digibook.Domain.Appointments;
using BlastAsia.Digibook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Infrastracture.Persistence.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(IDigiBookDbContext context):base(context) { }
    }
}
