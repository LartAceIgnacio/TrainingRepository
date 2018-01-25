using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class PilotRepository
        : RepositoryBase<Pilot>, IPilotRepository
    {
        public PilotRepository(IDigiBookDbContext context) : base(context)
        {

        }
        public Pagination<Pilot> Retrieve(int pageNo, int numRec, string filterValue)
        {
            Pagination<Pilot> result = new Pagination<Pilot>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Pilot>().OrderBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Pilot>().Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }

                return result;
            }
            else
            {
                result.Results = context.Set<Pilot>()
                    .Where(x => x.LastName.ToLower().Contains(filterValue.ToLower())
                    || x.FirstName.ToLower().Contains(filterValue.ToLower())
                    || x.MiddleName.ToLower().Contains(filterValue.ToLower())
                    || x.PilotCode.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                    if (result.Results.Count > 0)
                    {
                        result.TotalRecords = context.Set<Pilot>().Where(x => x.LastName.ToLower().Contains(filterValue.ToLower()))
                            .OrderBy(x => x.LastName).Count();
                        result.PageNo = pageNo;
                        result.RecordPage = numRec;
                    }

                    return result;
            }
        }

        public Pilot Retrieve(string p)
        {
            return context.Set<Pilot>().FirstOrDefault(x => x.PilotCode.ToLower().Contains(p.ToLower()));
        }
    }
}