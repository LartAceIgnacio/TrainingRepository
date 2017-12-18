using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository 
        : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(IDigiBookDbContext context)
            :base(context)
        {

        }
    }
}
