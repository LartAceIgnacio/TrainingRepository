using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Domain;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class VenueRepository : RepositoryBase<Venue>, IVenueRepository
    {
        private DigiBookDbContext context;

        public VenueRepository(DigiBookDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}