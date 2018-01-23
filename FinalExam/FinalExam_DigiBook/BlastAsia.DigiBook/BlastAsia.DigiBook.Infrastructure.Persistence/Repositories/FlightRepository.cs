using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Flights;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class FlightRepository
        : RepositoryBase<Flight>, IFlightRepository
    {
        public FlightRepository(IDigiBookDbContext context)
            : base(context)
        {
        }
        public Pagination<Flight> Retrieve(int pageNo, int numRec, string filterValue)
        {
            Pagination<Flight> result = new Pagination<Flight>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Flight>().OrderBy(x => x.ExpectedTimeOfDeparture)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Flight>().Count();
                    result.PageNo = pageNo;
                    result.PageRecord = numRec;
                }

                return result;
            }
            else
            {
                result.Results = context.Set<Flight>()
                    .Where(x => x.CityOfOrigin.ToLower().Contains(filterValue.ToLower()) ||
                    x.CityOfDestination.ToLower().Contains(filterValue.ToLower()) ||
                    x.FlightCode.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(x => x.ExpectedTimeOfDeparture)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Flight>()
                    .Where(x => x.CityOfOrigin.ToLower().Contains(filterValue.ToLower()) ||
                    x.CityOfDestination.ToLower().Contains(filterValue.ToLower()) ||
                    x.FlightCode.ToLower().Contains(filterValue.ToLower())).Count();
                    result.PageNo = pageNo;
                    result.PageRecord = numRec;
                }

                return result;
            }
        }
    }
}