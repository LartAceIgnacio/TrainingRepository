using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public interface IPilotRepository
        : IRepository<Pilot>
    {
        //Pilot Create(Pilot pilot);
        Pilot Retrieve(string code);
        Pagination <Pilot> Retrieve(int pageNumber, int recordNumber, string keyWord);
        //Pilot Retrieve(Guid id);
        //Pilot Update(Pilot pilot);
    }
}