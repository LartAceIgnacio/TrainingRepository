using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        private readonly IDigiBookDbContext context;
        public AppointmentRepository(IDigiBookDbContext context) : base(context)
        {
            this.context = context;
        }

        public Pagination<Appointment> Retrieve(int pageNumber, int recordNumber, string searchKey)
        {
            Pagination<Appointment> result = new Pagination<Appointment>()
            {
                PageNumber = pageNumber < 0 ? 1 : pageNumber,
                RecordNumber = recordNumber < 0 ? 1 : recordNumber,
                TotalCount = this.context.Set<Appointment>().Count()
            };

            if (pageNumber < 0)
            {
                result.Result = this.context.Set<Appointment>().OrderBy(c => c.AppointmentDate)
                    .Skip(0).Take(10).ToList();

                return result;
            }
            if (recordNumber < 0)
            {
                result.Result = this.context.Set<Appointment>().OrderBy(c => c.AppointmentDate)
                    .Skip(0).Take(10).ToList();

                return result;
            }
            else
            {
                result.Result = this.context.Set<Appointment>().OrderBy(c => c.AppointmentDate)
                   .Skip(pageNumber).Take(recordNumber).ToList();

                return result;
            }
        }
    }
}