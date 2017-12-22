using BlastAsia.Digibook.Domain.Models.Venues;
using BlastAsia.Digibook.Domain.Venues;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Infrastracture.Persistence.Repositories
{
    public class VenueRepository : RepositoryBase<Venue>, IVenueRepository
    {
        public VenueRepository(IDigiBookDbContext context) : base(context) { }
    }
}
