using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using System.Collections.Generic;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class VenueRepository
        : RepositoryBase<Venue>, IVenueRepository
    {
        public VenueRepository(IDigiBookDbContext context)
            : base(context)
        {
        }

        public PaginationResult<Venue> Retrieve(int pageNo, int numRec, string filterValue)
        {
            PaginationResult<Venue> result = new PaginationResult<Venue>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Venue>().OrderBy(x => x.VenueName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Venue>().Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }

                return result;
            }
            else
            {
                result.Results = context.Set<Venue>().Where(x => x.VenueName.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(x => x.VenueName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Venue>().Where(x => x.VenueName.ToLower().Contains(filterValue.ToLower()))
                        .OrderBy(x => x.VenueName).Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }

                return result;
            }
        }
    }
}