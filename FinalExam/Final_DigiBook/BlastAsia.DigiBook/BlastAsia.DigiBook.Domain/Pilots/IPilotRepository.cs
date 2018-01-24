using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Models.Records;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public interface IPilotRepository
        : IRepository<Pilot>
    {
        Record<Pilot> Fetch(int pageNo, int numRec, string filterValue);
        Pilot Retrieve(string pilotCode);
    }
}