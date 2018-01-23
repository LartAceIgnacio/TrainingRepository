using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class PilotRepository
         : RepositoryBase<Pilot>, IPilotRepository
    {
        public PilotRepository(IDigiBookDbContext context)
            : base(context)
        {
        }

        //public Pilot Create(Pilot pilot)
        //{
        //    context.Set<Pilot>().Add(pilot);
        //    context.SaveChanges();
        //    return pilot;
        //}

        //public void Delete(Guid pilotId)
        //{
        //    var found = this.Retrieve(pilotId);
        //    context.Set<Pilot>().Remove(found);
        //    context.SaveChanges();
        //}

        //public Pilot Retrieve(Guid pilotId)
        //{
        //    return context.Set<Pilot>().Find(pilotId);
        //}

        //public IEnumerable<Pilot> Retrieve()
        //{
        //    throw new NotImplementedException();
        //}

        ////public IEnumerable<Pilot> Retrieve()
        ////{
        ////    throw new NotImplementedException();
        ////}

        //public Pilot Update(Guid id, Pilot pilot)
        //{
        //    context.SaveChanges();
        //    return pilot;
        //}
    }
}