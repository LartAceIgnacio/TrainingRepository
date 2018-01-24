using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Repositories
{
    public class PilotRepository
           : RepositoryBase<Pilot>, IPilotRepository
    {
        private readonly IDigiBookDbContext context;
        public PilotRepository(IDigiBookDbContext context) : base(context)
        {
            this.context = context;
        }

        public Pilot Retrieve(string code)
        {
            return this.context.Set<Pilot>().FirstOrDefault(i => i.PilotCode == code);
        }

        public Pagination<Pilot> Retrieve(int pageNumber, int recordNumber, string keyWord)
        {
            //var pilots = new List<PilotSearchResult>();
            //var result = new Pagination<PilotSearchResult>() {
            //    PageNumber = pageNumber,
            //    RecordNumber = recordNumber,
            //};

            //var found = this.context.Set<Pilot>()
            //    .Where(c => c.FirstName.Contains(keyWord) || c.MiddleName.Contains(keyWord) || c.LastName.Contains(keyWord) || c.PilotCode.Contains(keyWord))
            //    .ToList();

            //foreach (var item in found)
            //{
            //    var ctr = new PilotSearchResult()
            //    {
            //        FirstName = item.FirstName,
            //        MiddleInitial = item.MiddleName.Substring(0,1),
            //        LastNmae = item.LastName,
            //        PilotCode = item.PilotCode,
            //        DateActivated = item.DateActivated
            //    };
            //    pilots.Add(ctr);
            //}
            //result.Result = pilots;
            //return result;

            //Pagination<Pilot> result = new Pagination<Pilot>
            //{
            //    PageNumber = pageNumber,
            //    RecordNumber = recordNumber,
            //    TotalCount = this.context.Set<Pilot>().Count()
            //};

            var result = new Pagination<Pilot>()
            {
                PageNumber = pageNumber,
                RecordNumber = recordNumber,
                TotalCount = this.context.Set<Pilot>().Count()
            };


            if (pageNumber < 0)
            {
                result.Result = this.context.Set<Pilot>().Skip(0).Take(10).OrderBy(c => c.LastName).ToList();

                return result;
            }

            if (recordNumber < 0)
            {
                result.Result = this.context.Set<Pilot>().Skip(0).Take(10).OrderBy(c => c.LastName).ToList();
                return result;
            }

            if (string.IsNullOrEmpty(keyWord))
            {
                result.Result = this.context.Set<Pilot>().OrderBy(c => c.LastName)
                                                            .Skip(pageNumber)
                                                            .Take(recordNumber)
                                                            .ToList();
                return result;
            }
            else
            {
                result.Result = this.context.Set<Pilot>().Where(c => c.FirstName.Contains(keyWord) || c.MiddleName.Contains(keyWord) || c.LastName.Contains(keyWord) || c.PilotCode.Contains(keyWord))
                                                            .OrderBy(r => r.LastName)
                                                            .Skip(pageNumber)
                                                            .Take(recordNumber)
                                                            .ToList();
                result.TotalCount = result.Result.Count();
                return result;
            }
        }

        //private List<PilotSearchResult> GetSpecificInfo(List<Pilot> pilotLists) {

        //    var pilots = new List<PilotSearchResult>();

        //    foreach (var item in pilotLists)
        //    {
        //        var ctr = new PilotSearchResult()
        //        {
        //            FirstName = item.FirstName,
        //            MiddleInitial = item.MiddleName.Substring(0, 1),
        //            LastNmae = item.LastName,
        //            PilotCode = item.PilotCode,
        //            DateActivated = item.DateActivated
        //        };
        //        pilots.Add(ctr);
        //    }

        //    return pilots;
        //}
    }
}