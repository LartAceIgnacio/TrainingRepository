using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        private IDigiBookDbContext context;
        public AppointmentRepository(IDigiBookDbContext context)
            : base(context)
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
                result.Results = this.context.Set<Appointment>()
                    .Skip(0)
                    .Take(10)
                    .OrderBy(c => c.AppointmentDate)
                    .ToList();

                return result;
            }

            if (recordNumber < 0)
            {
                result.Results = this.context.Set<Appointment>()
                    .Skip(0)
                    .Take(10)
                    .OrderBy(c => c.AppointmentDate)
                    .ToList();

                return result;
            }

            if (date == null)
            {
                result.Results = this.context.Set<Appointment>()
                    .Skip(pageNumber)
                    .Take(recordNumber)
                    .OrderBy(c => c.AppointmentDate)
                    .ToList();

                return result;
            }
            else
            {
                result.Results = this.context.Set<Appointment>()
                    .Where(c => c.AppointmentDate == date)
                    .Skip(pageNumber)
                    .Take(recordNumber)
                    .OrderBy(c => c.AppointmentDate)
                    .ToList();

                result.TotalCount = result.Results.Count();

                return result;
            }
        }
    }
}
