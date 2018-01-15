using BlastAsia.DigiBook.Domain.Models.Records;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class VenueRepository
       : RepositoryBase<Venue>, IVenueRepository
    {
      
        public VenueRepository(IDigiBookDbContext context)
            : base(context)
        {
        }

        public Record<Venue> Pagination(int page, int record, string filter)
        {
            var result = new Record<Venue>();

            result.PageNo = page;
            result.RecordPage = record;
            result.Result = context.Set<Venue>().OrderBy(x => x.VenueName)
                .Skip(page).Take(record).ToList();
            result.TotalRecord = context.Set<Venue>().Count();
            return result;
        }
    }
}