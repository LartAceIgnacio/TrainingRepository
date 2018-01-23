using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class EmployeeRepository
        :RepositoryBase<Appointement>, IAppointmentRepository
    {
        public EmployeeRepository(IDigiBookDbContext context)
            :base(context)
        {

        }
    }
}
