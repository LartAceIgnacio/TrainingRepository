using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class FlightRepository : RepositoryBase<Flight>, IFlightRepository
    {
        public FlightRepository(IDigiBookDbContext context): base(context)
        {

        }

        public PaginationResult<Flight> Retrieve(int numberOfOffset, int numOfRecordPerPage, string filterValue)
        {
            PaginationResult<Flight> result = new PaginationResult<Flight>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Flight>().OrderBy(f => f.CityOfOrigin)
                    .Skip(numberOfOffset).Take(numOfRecordPerPage).ToList();
                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Flight>().Count();
                    result.PageNo = numberOfOffset;
                    result.RecordPage = numOfRecordPerPage;
                }
                return result;
            }
            else
            {
                result.Results = context.Set<Flight>().Where(
                    f => f.CityOfOrigin.ToLower().Contains(filterValue.ToLower())
                    || f.CityOfDestination.ToLower().Contains(filterValue.ToLower())
                    || f.FlightCode.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(f => f.CityOfOrigin)
                    .Skip(numberOfOffset).Take(numOfRecordPerPage).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Flight>().Where(
                        f => f.CityOfOrigin.ToLower().Contains(filterValue.ToLower())
                        || f.CityOfDestination.ToLower().Contains(filterValue.ToLower())
                        || f.FlightCode.ToLower().Contains(filterValue.ToLower()))
                        .Count();
                    result.PageNo = numberOfOffset;
                    result.RecordPage = numOfRecordPerPage;
                }
                return result;
            }
        }
    }
}
