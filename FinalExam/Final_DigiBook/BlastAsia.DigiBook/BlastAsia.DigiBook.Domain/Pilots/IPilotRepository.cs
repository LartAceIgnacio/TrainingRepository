using System;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public interface IPilotRepository
    {
        Pilot Create(Pilot pilot);
        Pilot Update(Guid id, Pilot pilot);
        Pilot Retrieve(Guid id);
    }
}