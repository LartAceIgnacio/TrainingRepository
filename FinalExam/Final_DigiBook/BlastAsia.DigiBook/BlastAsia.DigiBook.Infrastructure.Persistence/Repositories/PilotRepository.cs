using System;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class PilotRepository
    {
        private DigiBookDbContext context;

        public PilotRepository(DigiBookDbContext context)
        {
            this.context = context;
        }
        public Pilot Create(Pilot pilot)
        {
            context.Set<Pilot>().Add(pilot);
            context.SaveChanges();
            return pilot;
        }

        public void Delete(Guid pilotId)
        {
            var found = this.Retrieve(pilotId);
            context.Set<Pilot>().Remove(found);
            context.SaveChanges();
        }

        public Pilot Retrieve(Guid pilotId)
        {
            return context.Set<Pilot>().Find(pilotId);
        }

        public Pilot Update(Guid id, Pilot pilot)
        {
            context.SaveChanges();
            return pilot;
        }
    }
}