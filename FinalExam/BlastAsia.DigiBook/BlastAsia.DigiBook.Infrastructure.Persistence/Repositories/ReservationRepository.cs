using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Reservations;
using BlastAsia.DigiBook.Domain.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class ReservationRepository: RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(IDigiBookDbContext context): base(context)
        {

        }

        public PaginationResult<Reservation> Retrieve(int skipPerPage, int recordsPerPage, string filterValue)
        {
            PaginationResult<Reservation> result = new PaginationResult<Reservation>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Reservation>()
                    .OrderBy(x => x.VenueName)
                    .Skip(skipPerPage)
                    .Take(recordsPerPage)
                    .ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Reservation>().Count();
                    result.PageNo = skipPerPage;
                    result.RecordPage = recordsPerPage;
                }

                return result;
            }
            else
            {
                result.Results = context.Set<Reservation>().Where(x => x.VenueName.ToLower()
                .Contains(filterValue.ToLower()))
                .OrderBy(x => x.VenueName)
                .Skip(skipPerPage)
                .Take(recordsPerPage)
                .ToList();

                if(result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Reservation>()
                        .Where(x => x.VenueName.ToLower()
                        .Contains(filterValue.ToLower()))
                        .Count();
                    result.PageNo = skipPerPage;
                    result.RecordPage = recordsPerPage;
                }

                return result;
            }
        }
    }
}
