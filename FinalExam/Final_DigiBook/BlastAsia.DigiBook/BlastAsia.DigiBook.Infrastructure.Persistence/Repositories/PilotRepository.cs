using System;
using System.Collections.Generic;
using System.Linq;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Models.Records;
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

        public Record<Pilot> Fetch(int pageNo, int numRec, string filterValue)
        {
            Record<Pilot> fetchResult = new Record<Pilot>();
            if (string.IsNullOrEmpty(filterValue))
            {
                fetchResult.Result = context.Set<Pilot>().OrderBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (fetchResult.Result.Count > 0)
                {
                    fetchResult.TotalRecord = context.Set<Pilot>().Count();
                    fetchResult.PageNo = pageNo;
                    fetchResult.RecordPage = numRec;
                }
                return fetchResult;
            }
            else
            {
                var filterValueLower = filterValue.ToLower();
                fetchResult.Result = context.Set<Pilot>().Where(x => x.FirstName.ToLower().Contains(filterValueLower)
                    || x.LastName.ToLower().Contains(filterValueLower) || x.MiddleName.ToLower().Contains(filterValueLower)
                    || x.PilotCode.ToLower().Contains(filterValue))
                    .OrderBy(x => x.FirstName)
                    .ToList();

                if (fetchResult.Result.Count > 0)
                {
                    fetchResult.TotalRecord = fetchResult.Result.Count();
                    fetchResult.PageNo = pageNo;
                    fetchResult.RecordPage = numRec;
                }
                return fetchResult;
            }
        }

        public Pilot Retrieve(string pilotCode)
        {
            return context.Set<Pilot>().FirstOrDefault(x => x.PilotCode.ToLower().Contains(pilotCode));
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