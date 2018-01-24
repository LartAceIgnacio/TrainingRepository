using BlastAsia.DigiBook.Domain.Airports;
using BlastAsia.DigiBook.Domain.Models.Airports;
using BlastAsia.DigiBook.Domain.Models.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class AirportRepository : IAirportRepository
    {
        
        public Pagination<Airport> pagination(int pageNumber, int recordNumber, string query)
        {
            List<Airport> airport = new List<Airport>();
            Pagination<Airport> result = new Pagination<Airport>
            {
                PageNumber = pageNumber,
                RecordNumber = recordNumber,
                TotalCount = airport.Count()
            };

            if (pageNumber < 0)
            {
                result.Results = airport
                    .Skip(0)
                    .Take(10)
                    .OrderBy(c => c.Code)
                    .ToList();
                return result;
            }
            if (recordNumber < 0)
            {
                result.Results = airport
                    .Skip(0)
                    .Take(10)
                    .OrderBy(c => c.Code)
                    .ToList();
                return result;
            }
            if (string.IsNullOrEmpty(query))
            {
                result.Results = airport
                    .OrderBy(c => c.Code)
                    .Skip(pageNumber)
                    .Take(recordNumber)
                    .ToList();
                return result;
            }
            else
            {
                result.Results = airport
                    .Where(c => c.Code.Contains(query) || c.Name.Contains(query))
                    .Skip(pageNumber)
                    .Take(recordNumber)
                    .ToList()
                    .OrderBy(c => c.Code);

                result.TotalCount = result.Results.Count();
                return result;
            }
        }
    }
}
