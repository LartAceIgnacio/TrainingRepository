using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Repositories
{
    public class AppointmentRepository
        : RepositoryBase<Appointment>, IAppointmentRepository
    {
        private readonly IDigiBookDbContext context;

        public AppointmentRepository(IDigiBookDbContext context) : base(context)
        {
            this.context = context;
        }

        public Pagination<Appointment> Retrieve(int pageNumber, int recordNumber, DateTime? date)
        {

            Pagination<Appointment> result = new Pagination<Appointment>
            {
                PageNumber = pageNumber,
                RecordNumber = recordNumber,
                TotalCount = this.context.Set<Appointment>().Count()
            };

            if (pageNumber < 0)
            {
                result.Result = this.context.Set<Appointment>().Skip(0).Take(10).OrderBy(c => c.AppointmentDate).ToList();
                return result;
            }

            if (recordNumber < 0)
            {
                result.Result = this.context.Set<Appointment>().Skip(0).Take(10).OrderBy(c => c.AppointmentDate).ToList();
                return result;
            }

            if (date == null)
            {
                result.Result = this.context.Set<Appointment>().Skip(pageNumber).OrderBy(c => c.AppointmentDate)
                                                  .Take(recordNumber)
                                                  .ToList();
                return result;
            }
            else
            {
                result.Result = this.context.Set<Appointment>().Where(r => r.AppointmentDate == date).OrderBy(c => c.AppointmentDate)
                                                  .Skip(pageNumber)
                                                  .Take(recordNumber)
                                                  .ToList();
                result.TotalCount = result.Result.Count();
                return result;
            }
           
        }
    }
}
