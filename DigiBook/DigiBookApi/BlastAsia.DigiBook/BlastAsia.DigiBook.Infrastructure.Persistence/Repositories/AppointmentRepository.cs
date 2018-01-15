using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public PaginationClass<Appointment> Retrieve(int pageNo, int numRec, string filterValue)
        {
            PaginationClass<Appointment> result = new PaginationClass<Appointment>();
            var c = Convert.ToDateTime(filterValue);
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Appointment>().OrderBy(x => x.AppointmentDate)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Appointment>().Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }
            }
            else
            {
                result.Results = context.Set<Appointment>().Where(x => x.AppointmentDate.ToString().Equals(filterValue))
                    .OrderBy(x => x.AppointmentDate)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Appointment>().Where(x => x.AppointmentDate.ToString().Equals(filterValue))
                    .OrderBy(x => x.AppointmentDate).Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }
            }

            return result;
        }
    }
}
