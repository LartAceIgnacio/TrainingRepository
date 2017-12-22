using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using System;
using System.Collections.Generic;
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
    }
}
