using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class VenueRepository
        : RepositoryBase<Venue>, IVenueRepository
    {
        public VenueRepository(IDigiBookDbContext context)
            : base(context)
        {

        }
        public Pagination<Venue> Retrieve(int pageNo, int numRec, string filterValue)
        {
            Pagination<Venue> result = new Pagination<Venue>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Venue>().OrderBy(x => x.VenueName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Venue>().Count();
                    result.PageNo = pageNo;
                    result.PageRecord = numRec;
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
                    result.PageRecord = numRec;
                }

                return result;
            }
        }
    }
}
