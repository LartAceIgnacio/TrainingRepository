using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public interface IPilotRepository
        : IRepository<Pilot>
    {
        //Pilot Create(Pilot pilot);
        Pilot Retrieve(string code);
        IEnumerable<Pilot> Search(string key);
        //Pilot Retrieve(Guid id);
        //Pilot Update(Pilot pilot);
    }
}