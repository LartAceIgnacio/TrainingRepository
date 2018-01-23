using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Records;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository
      : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(IDigiBookDbContext context)
            : base(context)
        {
        }
        public Record<Appointment> Fetch(int pageNo, int numRec, string filterValue)
        {
            Record<Appointment> fetchResult = new Record<Appointment>();
            if (string.IsNullOrEmpty(filterValue))
            {
                fetchResult.Result = context.Set<Appointment>().OrderBy(x => x.AppointmentDate)
                    .Skip(pageNo).Take(numRec).ToList();

                if (fetchResult.Result.Count > 0)
                {
                    fetchResult.TotalRecord = context.Set<Appointment>().Count();
                    fetchResult.PageNo = pageNo;
                    fetchResult.RecordPage = numRec;
                }
                return fetchResult;
            }
            else
            {
                fetchResult.Result = context.Set<Appointment>().Where(x => x.AppointmentDate.ToString().Equals(filterValue)
                    || x.Host.FirstName.ToLower().Contains(filterValue.ToLower()) || x.Host.LastName.ToLower().Contains(filterValue.ToLower())
                    || x.Guest.FirstName.ToLower().Contains(filterValue.ToLower()) || x.Guest.LastName.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(x => x.AppointmentDate)
                    .ToList();

                if (fetchResult.Result.Count > 0)
                {
                    fetchResult.TotalRecord = context.Set<Appointment>().Where(x => x.AppointmentDate.ToString().Equals(filterValue.ToLower()))
                    .OrderBy(x => x.AppointmentDate).Count();
                    fetchResult.PageNo = pageNo;
                    fetchResult.RecordPage = numRec;
                }
                return fetchResult;
            }
        }
    }
}